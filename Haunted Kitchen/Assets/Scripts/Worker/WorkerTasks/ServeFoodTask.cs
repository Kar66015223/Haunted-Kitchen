using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServeFoodTask : IWorkerTask, ITaskReceiver
{
    private List<IWorkerInteractable> discoveredItems = new();
    private IWorkerInteractable targetItem;
    private bool changeTarget = false;
    
    public string TaskName => WorkerConstants.TASK_SERVEFOOD_NAME;
    public int Priority => WorkerConstants.TASK_SERVEFOOD_PRIORITY;

    public void OnTargetDiscovered(IWorkerInteractable target)
    {
        if (target is Item && !discoveredItems.Contains(target) && !target.IsTargeted)
        {
            discoveredItems.Add(target);

            target.OnFinished += OnFinished;

            if (targetItem == null)
            {
                targetItem = target;
                target.IsTargeted = true;
            }
        }
    }
    
    private void OnFinished(IWorkerInteractable target)
    {
        if (target == targetItem)
        {
            targetItem = null;
        }

        discoveredItems.Remove(target);
        target.OnFinished -= OnFinished;
    }

    public bool CanExecute(WorkerContext context) =>
        targetItem != null &&
        IsTargetValid(targetItem);

    public void Start(WorkerContext context)
    {
        context.CurrentTarget = targetItem.GetPosition();
        context.Agent.SetDestination(context.CurrentTarget.position);
    }

    public void Update(WorkerContext context)
    {
        if (!IsTargetValid(targetItem))
        {
            if (targetItem != null)
            {
                discoveredItems.Remove(targetItem);
                targetItem = null;
                context.CurrentTarget = null;
            }

            changeTarget = true;
            return;
        }

        changeTarget = false;
        discoveredItems.RemoveAll(item => !IsTargetValid(item) || item.IsTargeted);

        if (IsComplete(context))
            End(context);
    }

    public void End(WorkerContext context)
    {
        if (targetItem != null && context.CurrentTarget != null)
        {
            Item item = context.CurrentTarget.gameObject.GetComponent<Item>();

            if (item != null)
            {
                Debug.Log("Picked up this item");
            }

            discoveredItems.Remove(targetItem);
            context.CurrentTarget = null;
            targetItem = null;
        }

        targetItem = discoveredItems.FirstOrDefault();

        if (targetItem != null)
            targetItem.IsTargeted = true;
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
