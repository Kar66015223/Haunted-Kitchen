using UnityEngine;

public interface ITaskReceiver
{
    void OnTargetDiscovered(IWorkerInteractable target);
}
