using UnityEngine;

public interface IWorkerTask
{
    string TaskName { get; }
    int Priority { get; }

    bool CanExecute(WorkerContext context);
    void Start(WorkerContext context);
    void Update(WorkerContext context);
    void End(WorkerContext context);
    bool IsComplete(WorkerContext context);
}
