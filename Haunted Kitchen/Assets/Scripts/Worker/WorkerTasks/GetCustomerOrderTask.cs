using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;

public class GetCustomerOrderTask : IWorkerTask, ITaskReceiver
{
    private List<IWorkerInteractable> discoveredCustomers = new();
    private IWorkerInteractable targetCustomer;
    // private bool changeTarget = false;

    public string TaskName => WorkerConstants.TASK_GETCUSTOMERORDER_NAME;
    public int Priority => WorkerConstants.TASK_GETCUSTOMERORDER_PRIORITY;

    public void OnTargetDiscovered(IWorkerInteractable target)
    {
        if (target is Customer_New customer &&
        !discoveredCustomers.Contains(target) /* && */
        /* target.Claimer == null */)
        {
            discoveredCustomers.Add(target);

            customer.OnOrderTaken += OnFinished;
            customer.OnFinished += _ => discoveredCustomers.Remove(customer);

            // if (target is Customer_New customer &&
            // customer.GetCurrentState() == CustomerState.Idle)
            // {
            // }

            // if (targetCustomer == null)
            // {
            //     targetCustomer = target;
            //     target.TrySetClaimer(this);
            // }

            // Debug.Log($"Found {target}. Discovered Customers: {discoveredCustomers.Count}");
        }
    }

    private void RefreshTarget()
    {
        discoveredCustomers.RemoveAll(customer => !IsTargetValid(customer));

        targetCustomer = discoveredCustomers.FirstOrDefault(c =>
            c is Customer_New customer &&
            customer.GetCurrentState() == CustomerState.Idle &&
            (customer.Claimer == null || customer.Claimer == this));
    }

    public bool CanExecute(WorkerContext context)
    {
        RefreshTarget();
        return targetCustomer != null;
        // Customer_New customer = targetCustomer as Customer_New;

        // return targetCustomer != null &&
        //     customer.GetCurrentState() == CustomerState.Idle &&
        //     IsTargetValid(targetCustomer);
    }

    public void Start(WorkerContext context)
    {
        // if (targetCustomer == null)
        //     return;

        // if (!targetCustomer.TrySetClaimer(this))
        // {
        //     targetCustomer = null;
        //     return;
        // }

        if(targetCustomer == null || !targetCustomer.TrySetClaimer(this))
        {
            RefreshTarget();
            if (targetCustomer == null || !targetCustomer.TrySetClaimer(this))
                return;
        }

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
                // discoveredCustomers.Remove(targetCustomer);
                // targetCustomer.ClearClaimer(this);
                targetCustomer = null;
                return;
                // context.CurrentTarget = null;
            }

            // Debug.Log("Current target is not valid");
            // changeTarget = true;
            // return;
        }

        // changeTarget = false;
        // discoveredCustomers.RemoveAll(customer => !IsTargetValid(customer));

        // if (IsComplete(context))
        //     End(context);
    }

    public void End(WorkerContext context)
    {
        if(targetCustomer != null && IsComplete(context))
        {
            if(targetCustomer is Customer_New customer)
            {
                customer.Order();
            }

            // customer.ClearClaimer(this);
            // discoveredCustomers.Remove(targetCustomer);
        }

        // context.CurrentTarget = null;
        targetCustomer = null;
        RefreshTarget();

        // targetCustomer = discoveredCustomers.FirstOrDefault();

        // if (targetCustomer != null)
        // {
        //     Customer_New customer = targetCustomer as Customer_New;
        //     if (customer.GetCurrentState() == CustomerState.Idle)
        //     {
        //         Start(context);
        //     }
        // }

        // Debug.Log($"Task ended, Next target customer: {targetCustomer}");
    }

    public bool IsComplete(WorkerContext context)
    {
        if (targetCustomer == null)
            return true;
 
        return !context.Agent.pathPending &&
            context.Agent.remainingDistance <= context.Agent.stoppingDistance;
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
            customer.OnOrderTaken -= OnFinished;
        }
    }

    private bool IsTargetValid(IWorkerInteractable target)
    {
        if (target == null)
            return false;

        if (target is MonoBehaviour mb && mb == null)
            return false;

        if (target is MonoBehaviour mono && !mono.gameObject.activeInHierarchy)
            return false;

        // if(target is Customer_New customer)
        // {
        //     if (customer.GetCurrentState() != CustomerState.Idle)
        //         return false;

        //     if (customer.Claimer != null && customer.Claimer != this)
        //         return false;
        // }

        return true;
    }
}
