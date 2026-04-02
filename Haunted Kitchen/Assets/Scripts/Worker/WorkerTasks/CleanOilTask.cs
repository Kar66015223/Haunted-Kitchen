using UnityEngine;

public class CleanOilTask : IWorkerTask
{
    [SerializeField] private GameObject targetOil;
    private float cleanTime = WorkerConstants.TASK_CLEANOIL_CLEANTIME;
    private float elapsedTime;

    public string TaskName => WorkerConstants.TASK_CLEANOIL_NAME;
    public int Priority => WorkerConstants.TASK_CLEANOIL_PRIORITY;

    public bool CanExecute(WorkerContext context)
    {
        throw new System.NotImplementedException();
    }

    public void End(WorkerContext context)
    {
        throw new System.NotImplementedException();
    }

    public bool IsComplete(WorkerContext context)
    {
        throw new System.NotImplementedException();
    }

    public void Start(WorkerContext context)
    {
        throw new System.NotImplementedException();
    }

    public void Update(WorkerContext context)
    {
        throw new System.NotImplementedException();
    }
}
