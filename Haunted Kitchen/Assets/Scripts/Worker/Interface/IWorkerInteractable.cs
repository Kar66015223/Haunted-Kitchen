using UnityEngine;

public interface IWorkerInteractable
{
    bool IsTargeted { get; set; }
    
    void OnDiscovered();
    Transform GetPosition();
}
