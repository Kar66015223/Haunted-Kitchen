using System;
using UnityEngine;

public interface IWorkerInteractable
{
    bool IsTargeted { get; set; }
    event Action<IWorkerInteractable> OnFinished;

    void OnDiscovered();
    Transform GetPosition();
}
