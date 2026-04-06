using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServeFoodTask : IWorkerTask, ITaskReceiver
{
    private List<Customer_New> discoveredCustomers = new();
    private List<Item> discoveredItems;

    private OrderDelivery currentOrder;
    private Item currentItemTarget;

    private enum ServeState { Idle, PickingUp, Delivering, Complete }
    private ServeState serveState = ServeState.Idle;
    
    // private bool changeTarget = false;
    
    public string TaskName => WorkerConstants.TASK_SERVEFOOD_NAME;
    public int Priority => WorkerConstants.TASK_SERVEFOOD_PRIORITY;

    public void OnTargetDiscovered(IWorkerInteractable target)
    {
        if (target == null)
            return;
            
        // Customer discovery
        if (target is Customer_New customer)
        {
            if (!discoveredCustomers.Contains(customer))
            {
                discoveredCustomers.Add(customer);
                customer.OnFinished += OnCustomerFinished;

                Debug.Log($"Discovered customer: {customer.name}");
            }
        }

        // Item discovery
        if (target is Item item)
        {
            if (item.itemData == null)
            {
                Debug.LogWarning($"Item {item.name} has null itemData");
                return;
            }

            if (!(item.itemData is FoodData))
                return;

            if (!discoveredItems.Contains(item))
            {
                discoveredItems.Add(item);
                item.OnFinished += OnItemFinished;

                Debug.Log($"Discovered food: {item.itemData.itemName}");
            }
        }
    }

    private void OnCustomerFinished(IWorkerInteractable target)
    {
        if (target == null)
            return;

        discoveredCustomers.Remove((Customer_New)target);
        target.OnFinished -= OnCustomerFinished;
    }
    
    private void OnItemFinished(IWorkerInteractable target)
    {
        if (target == null)
            return;

        discoveredItems.Remove((Item)target);
        target.OnFinished -= OnCustomerFinished;
    }

    public bool CanExecute(WorkerContext context)
    {
        var validOrders = FindValidDeliveryOrders();
        return validOrders.Count > 0;
    }

    public void Start(WorkerContext context)
    {
        var validOrders = FindValidDeliveryOrders();
        currentOrder = validOrders.FirstOrDefault();

        if(currentOrder != null)
        {
            currentItemTarget = currentOrder.availableItems.First();
            context.CurrentTarget = currentItemTarget.GetPosition();
            context.Agent.SetDestination(context.CurrentTarget.position);

            serveState = ServeState.PickingUp;
            currentOrder.customer.IsTargeted = true;
            currentItemTarget.IsTargeted = true;

            Debug.Log($"Starting delivery: {currentItemTarget.itemData.itemName} -> {currentOrder.customer.name}");
        }
    }

    public void Update(WorkerContext context)
    {
        if (currentOrder == null || !currentOrder.IsValid)
        {
            serveState = ServeState.Complete;
            return;
        }

        switch (serveState)
        {
            case ServeState.PickingUp:
                UpdatePickingUp(context);
                break;
            case ServeState.Delivering:
                UpdateDelivering(context);
                break;
        }
    }

    private void UpdatePickingUp(WorkerContext context)
    {
        if (!context.Agent.pathPending &&
            context.Agent.remainingDistance <= context.Agent.stoppingDistance)
        {
            PickupItem(currentItemTarget);

            if (currentOrder.availableItems.Count == 0)
            {
                serveState = ServeState.Delivering;
                context.CurrentTarget = currentOrder.customer.GetPosition();
                context.Agent.SetDestination(context.CurrentTarget.position);

                Debug.Log($"Item picked up, delivering to {currentOrder.customer.name}");
            }
            else
            {
                currentItemTarget = currentOrder.availableItems.First();
                context.CurrentTarget = currentItemTarget.GetPosition();
                context.Agent.SetDestination(context.CurrentTarget.position);

                Debug.Log($"Moving to next item: {currentItemTarget.itemData.itemName}");
            }
        }
    }
    
    private void UpdateDelivering(WorkerContext context)
    {
        if (!context.Agent.pathPending &&
            context.Agent.remainingDistance <= context.Agent.stoppingDistance)
        {
            ServeCustomer(currentOrder);
            serveState = ServeState.Complete;

            Debug.Log($"Order delivered to {currentOrder.customer.name}");
        }
    }

    public void End(WorkerContext context)
    {
        if (currentOrder != null)
        {
            currentOrder.customer.IsTargeted = false;
        }
        
        currentOrder = null;
        currentItemTarget = null;
        serveState = ServeState.Idle;
        context.CurrentTarget = null;
    }

    public bool IsComplete(WorkerContext context) => serveState == ServeState.Complete;

    private List<OrderDelivery> FindValidDeliveryOrders()
    {
        var orders = new List<OrderDelivery>();

        foreach (var customer in discoveredCustomers)
        {
            if (customer.GetCurrentState() != CustomerState.Ordered)
                continue;

            var orderedItems = customer.GetComponent<CustomerOrder>().GetOrderedItems();

            var matchingItems = FindMatchingItems(orderedItems);

            if (matchingItems.Count > 0)
            {
                orders.Add(new OrderDelivery
                {
                    customer = customer,
                    orderedItems = orderedItems,
                    availableItems = matchingItems
                });
            }
        }

        return orders
            .OrderBy(o => o.GetPatienceRemaining())
            .ToList();
    }

    private List<Item> FindMatchingItems(List<ItemData> orderedItems)
    {
        var matches = new List<Item>();

        if (discoveredItems != null)
        {
            foreach (var item in discoveredItems)
            {
                if (item.IsTargeted || !(item.itemData is FoodData))
                    continue;
    
                if (orderedItems.Any(ordered => ordered.itemName == item.itemData.itemName))
                {
                    matches.Add(item);
                }
            }
        }

        return matches;
    }

    private void PickupItem(Item item)
    {
        discoveredItems.Remove(item);
        currentOrder.availableItems.Remove(item);

        Object.Destroy(item.gameObject);

        Debug.Log($"Picking up: {item.itemData.itemName}");
    }
    
    private void ServeCustomer(OrderDelivery order)
    {
        var itemsServed = order.orderedItems; // Items we were supposed to deliver
        order.customer.WorkerOrderServe(itemsServed);
    }
}
