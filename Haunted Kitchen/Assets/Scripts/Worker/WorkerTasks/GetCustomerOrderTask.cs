using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GetCustomerOrderTask : IWorkerTask, ITaskReceiver
{
    private List<IWorkerInteractable> discoveredCustomers = new();
    private IWorkerInteractable targetCustomer;

    private bool changeTarget = false;

    public string TaskName => WorkerConstants.TASK_GETCUSTOMERORDER_NAME;
    public int Priority => WorkerConstants.TASK_GETCUSTOMERORDER_PRIORITY;

    public void OnTargetDiscovered(IWorkerInteractable target)
    {
        if (target is Customer_New &&
        !discoveredCustomers.Contains(target) &&
        !target.IsTargeted)
        {
            discoveredCustomers.Add(target);

            if (target is Customer_New customer)
            {
                target.OnFinished += OnFinished;
            }

            if (targetCustomer == null)
            {
                targetCustomer = target;
                target.IsTargeted = true;
            }

            // Debug.Log($"Found {target}. Discovered Customers: {discoveredCustomers.Count}");
        }
    }
    
    private void OnFinished(IWorkerInteractable target)
    {
        if (target == targetCustomer)
        {
            targetCustomer = null;
        }

        discoveredCustomers.Remove(target);

        if(target is Customer_New customer)
        {
            customer.OnFinished -= OnFinished;
        }
    }

    public bool CanExecute(WorkerContext context) =>
        targetCustomer != null &&
        IsTargetValid(targetCustomer);

    public void Start(WorkerContext context)
    {
        context.CurrentTarget = targetCustomer.GetPosition();
        context.Agent.SetDestination(context.CurrentTarget.position);

        // Debug.Log($"Walking to {context.CurrentTarget.gameObject.name}");
    }

    public void Update(WorkerContext context)
    {
        if (!IsTargetValid(targetCustomer))
        {
            if (targetCustomer != null)
            {
                discoveredCustomers.Remove(targetCustomer);
                targetCustomer = null;
                context.CurrentTarget = null;
            }

            // Debug.Log("Current target is not valid");
            changeTarget = true;

            return;
        }

        changeTarget = false;
        discoveredCustomers.RemoveAll(customer => !IsTargetValid(customer));

        if (IsComplete(context))
            End(context);
    }

    public void End(WorkerContext context)
    {
        if(targetCustomer != null && context.CurrentTarget != null)
        {
            Customer_New customer = context.CurrentTarget.gameObject.GetComponent<Customer_New>();

            if (customer != null && customer.GetCurrentState() != CustomerState.Ordered)
            {
                customer.Order();
            }

            discoveredCustomers.Remove(targetCustomer);
            context.CurrentTarget = null;
            targetCustomer = null;
        }

        targetCustomer = discoveredCustomers.FirstOrDefault();

        if (targetCustomer != null)
            targetCustomer.IsTargeted = true;

        // Debug.Log($"Task ended, Next target customer: {targetCustomer}");
    }

    public bool IsComplete(WorkerContext context) =>
        !context.Agent.pathPending &&
        context.Agent.remainingDistance <= context.Agent.stoppingDistance ||
        changeTarget;

    private bool IsTargetValid(IWorkerInteractable target)
    {
        if (target == null)
            return false;

        if (target is MonoBehaviour mb && mb == null)
            return false;

        if (target is MonoBehaviour mono && !mono.gameObject.activeInHierarchy)
            return false;

        return true;
    }
}
