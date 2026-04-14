using UnityEngine;
using System.Linq;

public class ServeFoodTask : IWorkerTask, ITaskReceiver
{
    private readonly ServeFoodTaskRegistry registry = new();
    private OrderDelivery currentOrder;
    private Item currentItem;
    private Customer_New currentCustomer;

    private ServeState state = ServeState.Idle;

    private enum ServeState { Idle, PickingUp, Delivering, Completed }

    public string TaskName => WorkerConstants.TASK_SERVEFOOD_NAME;
    public int Priority => WorkerConstants.TASK_SERVEFOOD_PRIORITY;

    private WorkerPickup pickup;

    public ServeFoodTask(WorkerPickup pickup)
    {
        this.pickup = pickup;
    }

    public void OnTargetDiscovered(IWorkerInteractable target) =>
        registry.Register(target);

    public bool CanExecute(WorkerContext context)
    {
        registry.Cleanup();
        return OrderMatcher.FindBestOrder(
            registry.Customers,
            registry.Items,
            currentItem,
            currentOrder?.customer,
            this) != null;
    }

    public void Start(WorkerContext context)
    {
        currentOrder = OrderMatcher.FindBestOrder(
            registry.Customers,
            registry.Items,
            currentItem,
            currentOrder?.customer,
            this);

        if (currentOrder == null || currentOrder.customer == null)
        {
            Debug.LogWarning("currentOrder == null || currentOrder.customer == null");
            state = ServeState.Completed;
            return;
        }

        currentCustomer = currentOrder.customer;

        if (!currentCustomer.TrySetClaimer(this))
        {
            Debug.LogWarning("!currentOrder.customer.TrySetClaimer(this)");

            currentOrder = null;
            currentCustomer = null;
            state = ServeState.Completed;
            return;
        }

        SetNextItemTarget(context);
        state = ServeState.PickingUp;
    }

    public void Update(WorkerContext context)
    {
        if (currentOrder == null || !currentOrder.IsValid)
        {
            if (currentCustomer != null)
                currentCustomer.ClearClaimer(this);

            state = ServeState.Completed;
            Debug.LogWarning("currentOrder is null or invalid");
            return;
        }

        if (currentCustomer != null && currentCustomer.Claimer != this)
        {
            Debug.LogWarning("This customer is claimed");
            currentCustomer = null;
            state = ServeState.Completed;
            return;
        }

        if (context.Agent.pathPending ||
        context.Agent.remainingDistance > context.Agent.stoppingDistance)
        {
            // Debug.LogWarning("Walking");
            return;
        }
        
        if(!context.Agent.hasPath)
        {
            // Debug.LogWarning("No path");
            UpdateMoveTarget(context, context.CurrentTarget);
            return;
        }

        if (context.Agent.remainingDistance <= context.Agent.stoppingDistance)
        {
            switch (state)
            {
                case ServeState.PickingUp:
                    HandlePickup(context);
                    break;
                case ServeState.Delivering:
                    HandleDelivery(context);
                    break;
                case ServeState.Completed:
                    Debug.Log("Task completed");
                    break;
            }
        }
    }

    private void HandlePickup(WorkerContext context)
    {
        if (currentItem == null)
        {
            Debug.LogWarning("[ServeFoodTask] CurrentItem is null during pickup");
            state = ServeState.Completed;
            return;
        }

        Debug.Log($"Picked up {currentItem}");

        pickup.PickUp(currentItem);
        currentOrder.availableItems.Remove(currentItem);

        state = ServeState.Delivering;
        UpdateMoveTarget(context, currentOrder.customer.GetPosition());
    }

    private void HandleDelivery(WorkerContext context)
    {
        Debug.Log($"Served {currentOrder.customer}");

        currentOrder.customer.WorkerOrderServe(currentItem);
        if (pickup.currentItem != null)
        {
            pickup.Serve(pickup.currentItem);
        }

        state = ServeState.Completed;
    }

    private void SetNextItemTarget(WorkerContext context)
    {
        currentItem = currentOrder.availableItems.FirstOrDefault();

        if (currentItem == null)
        {
            state = ServeState.Completed;
            return;
        }

        if (!currentItem.TrySetClaimer(this))
        {
            state = ServeState.Completed;
            return;
        }

        currentItem.TrySetClaimer(this);
        UpdateMoveTarget(context, currentItem.GetPosition());

        // Debug.Log($"Setting next item {currentItem}");
    }

    private void UpdateMoveTarget(WorkerContext context, Transform target)
    {
        context.CurrentTarget = target;

        context.Agent.ResetPath();
        context.Agent.SetDestination(target.position);

        // Debug.Log($"Moving to {target}");
    }

    public void End(WorkerContext context)
    {
        if (currentOrder?.customer != null)
            currentOrder.customer.ClearClaimer(this);

        if (currentOrder?.availableItems != null)
        {
            foreach (var item in currentOrder.availableItems)
            {
                if (item != null) item.ClearClaimer(this);
            }
        }

        if (currentItem != null)
            currentItem.ClearClaimer(this);

        currentOrder = null;
        currentItem = null;
        currentCustomer = null;
        state = ServeState.Idle;

        // Debug.Log("End called");
    }

    public bool IsComplete(WorkerContext context) =>
        state == ServeState.Completed;
}
