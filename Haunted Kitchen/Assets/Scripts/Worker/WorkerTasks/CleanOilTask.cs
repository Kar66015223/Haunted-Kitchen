using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CleanOilTask : IWorkerTask, ITaskReceiver
{
    private List<IWorkerInteractable> discoveredOils = new();
    private IWorkerInteractable targetOil;

    private float cleanTime = WorkerConstants.TASK_CLEANOIL_CLEANTIME;
    private float elapsedTime;

    public string TaskName => WorkerConstants.TASK_CLEANOIL_NAME;
    public int Priority => WorkerConstants.TASK_CLEANOIL_PRIORITY;

    private WorkerAnimation anim;

    public CleanOilTask(WorkerAnimation anim)
    {
        this.anim = anim;
    }

    public void OnTargetDiscovered(IWorkerInteractable target)
    {
        if (target is Oil && !discoveredOils.Contains(target) /* && target.Claimer == null */)
        {
            discoveredOils.Add(target);
            target.OnFinished += _ => discoveredOils.Remove(target);

            // if (target is Oil oil)
            // {
            //     target.OnFinished += OnFinished;
            // }

            // if (targetOil == null)
            // {
            //     targetOil = target;
            //     target.TrySetClaimer(this);
            // }

            // Debug.Log($"Found {target}, Discovered Oil: {discoveredOils.Count}");
        }
    }

    private void RefreshTarget()
    {
        discoveredOils.RemoveAll(oil => !IsTargetValid(oil));
        targetOil = discoveredOils.FirstOrDefault(oil => oil.Claimer == null || oil.Claimer == this);
    }

    public bool CanExecute(WorkerContext context)
    {
        RefreshTarget();
        return targetOil != null;
        // return targetOil != null &&
        // targetOil.Claimer == this &&
        // IsTargetValid(targetOil);
    }

    public void Start(WorkerContext context)
    {
        // if (targetOil == null)
        //     return;

        // if (!targetOil.TrySetClaimer(this))
        // {
        //     targetOil = null;
        //     return;
        // }

        if (targetOil == null || !targetOil.TrySetClaimer(this))
        {
            RefreshTarget();
            if (targetOil == null || !targetOil.TrySetClaimer(this))
                return;
        }

        context.CurrentTarget = targetOil?.GetPosition();
        context.Agent.SetDestination(context.CurrentTarget.position);
        elapsedTime = 0f;

        // Debug.Log($"Walking to {context.CurrentTarget.gameObject.name}");
    }

    public void Update(WorkerContext context)
    {
        // if (!IsTargetValid(targetOil))
        // {
        //     if (targetOil != null)
        //     {
        //         discoveredOils.Remove(targetOil);
        //         targetOil.ClearClaimer(this);
        //         targetOil = null;
        //         context.CurrentTarget = null;
        //     }
        // }

        // changeTarget = false;
        // discoveredOils.RemoveAll(oil => !IsTargetValid(oil) ||
        //     oil.Claimer == null || oil.Claimer != this);

        if (!IsTargetValid(targetOil) || targetOil.Claimer != this)
        {
            targetOil = null;
            return;
        }

        elapsedTime += Time.deltaTime;
        anim.SetClean(true);

        // if (IsComplete(context))
        //     End(context);
    }

    public void End(WorkerContext context)
    {
        if (targetOil != null && IsComplete(context))
        {
            if (targetOil is Oil oil)
            {
                oil.Clean();
                // oil.OnFinished -= OnFinished;
            }

            // if (oil != null)
            // {
            //     oil.Clean();
            //     oil.OnFinished -= OnFinished;
            // }

            // oil.ClearClaimer(this);
            // discoveredOils.Remove(targetOil);
        }

        // context.CurrentTarget = null;
        // targetOil = null;

        // Pick new target
        // targetOil = discoveredOils.FirstOrDefault();

        // if (targetOil != null)
        // {
        //     Oil oil = targetOil as Oil;
        //     if(targetOil != null && targetOil.Claimer != this)
        //     {
        //         targetOil.ClearClaimer(this);
        //         Start(context);
        //     }
        // }

        anim.SetClean(false);
        targetOil = null;
        RefreshTarget();

        // Debug.Log($"Task ended, next target oil: {targetOil}");
    }

    public bool IsComplete(WorkerContext context) => targetOil == null || elapsedTime >= cleanTime;

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