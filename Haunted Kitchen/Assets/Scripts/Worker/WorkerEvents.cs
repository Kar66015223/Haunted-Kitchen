using System;

public static class WorkerEvents
{
    public static event Action<IWorkerInteractable> OnTaskDiscovered;

    public static void NotifyTaskDiscovered(IWorkerInteractable target)
    {
        OnTaskDiscovered?.Invoke(target);
    }
}
