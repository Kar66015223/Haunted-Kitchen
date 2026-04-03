using System.Collections.Generic;
using UnityEngine;

public class CleanOilTask : IWorkerTask, ITaskReceiver
{
    private List<IWorkerInteractable> discoveredOils = new();
    private IWorkerInteractable targetOil;
    private float cleanTime = WorkerConstants.TASK_CLEANOIL_CLEANTIME;
    private float elapsedTime;

    public string TaskName => WorkerConstants.TASK_CLEANOIL_NAME;
    public int Priority => WorkerConstants.TASK_CLEANOIL_PRIORITY;
    
    public void OnTargetDiscovered(IWorkerInteractable target)
    {
        if(target is Oil && !discoveredOils.Contains(target))
        {
            discoveredOils.Add(target);

            if (targetOil == null)
                targetOil = target;
        }
    }

    public bool CanExecute(WorkerContext context) => targetOil != null;

    public void Start(WorkerContext context)
    {
        context.CurrentTarget = targetOil?.GetPosition();
        context.Agent.SetDestination(context.CurrentTarget.position);
        elapsedTime = 0f;
    }

    public void Update(WorkerContext context)
    {
        elapsedTime += Time.deltaTime;

        if(IsComplete(context))
            End(context);
    }

    public void End(WorkerContext context)
    {
        if (targetOil != null)
        {
            if (context.CurrentTarget != null)
            {
                Oil oil = context.CurrentTarget.gameObject.GetComponent<Oil>();

                if (oil != null)
                {
                    oil.Clean();
                    context.CurrentTarget = null;
                    targetOil = null;
                }
            }

            discoveredOils.Remove(targetOil);
            targetOil = discoveredOils.Count > 0 ? discoveredOils[0] : null;
        }
    }

    public bool IsComplete(WorkerContext context) => elapsedTime >= cleanTime;
}