using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CleanOilTask : IWorkerTask, ITaskReceiver
{
    private List<IWorkerInteractable> discoveredOils = new();
    private IWorkerInteractable targetOil;
    private float cleanTime = WorkerConstants.TASK_CLEANOIL_CLEANTIME;
    private float elapsedTime;

    private bool changeTarget = false;

    public string TaskName => WorkerConstants.TASK_CLEANOIL_NAME;
    public int Priority => WorkerConstants.TASK_CLEANOIL_PRIORITY;

    public void OnTargetDiscovered(IWorkerInteractable target)
    {
        if (target is Oil && !discoveredOils.Contains(target) && !target.IsTargeted)
        {
            discoveredOils.Add(target);

            if (target is Oil oil)
            {
                target.OnFinished += OnFinished;
            }

            if (targetOil == null)
            {
                targetOil = target;
                target.IsTargeted = true;
            }

            Debug.Log($"Found {target}, Discovered Oil: {discoveredOils.Count}");
        }
    }
    
    // If targetOil is destroyed, remove from List and set targetOil to null
    private void OnFinished(IWorkerInteractable target)
    {
        if (target == targetOil)
        {
            targetOil = null;
        }

        discoveredOils.Remove(target);

        if(target is Oil oil)
        {
            oil.OnFinished -= OnFinished;
        }
    }

    public bool CanExecute(WorkerContext context) =>
        targetOil != null &&
        IsTargetValid(targetOil);

    public void Start(WorkerContext context)
    {
        context.CurrentTarget = targetOil?.GetPosition();
        context.Agent.SetDestination(context.CurrentTarget.position);
        elapsedTime = 0f;

        Debug.Log($"Walking to {context.CurrentTarget.gameObject.name}");
    }

    public void Update(WorkerContext context)
    {
        if (!IsTargetValid(targetOil))
        {
            if (targetOil != null)
            {
                discoveredOils.Remove(targetOil);
                targetOil = null;
                context.CurrentTarget = null;
            }

            Debug.Log("Current target is not valid");
            changeTarget = true;

            return;
        }

        changeTarget = false;
        discoveredOils.RemoveAll(oil => !IsTargetValid(oil) || oil.IsTargeted);

        elapsedTime += Time.deltaTime;

        if (IsComplete(context))
            End(context);
    }

    public void End(WorkerContext context)
    {
        if (targetOil != null && context.CurrentTarget != null)
        {
            Oil oil = context.CurrentTarget.gameObject.GetComponent<Oil>();

            if (oil != null)
            {
                oil.Clean();
                oil.OnFinished -= OnFinished;
            }

            discoveredOils.Remove(targetOil);
            context.CurrentTarget = null;
            targetOil = null;
        }

        // Pick new target
        targetOil = discoveredOils.FirstOrDefault();

        if (targetOil != null)
            targetOil.IsTargeted = true;

        Debug.Log($"Task ended, next target oil: {targetOil}");
    }

    public bool IsComplete(WorkerContext context) => elapsedTime >= cleanTime || changeTarget;

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