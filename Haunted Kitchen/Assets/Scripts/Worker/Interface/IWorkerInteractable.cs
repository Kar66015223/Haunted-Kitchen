using System;
using UnityEngine;

public interface IWorkerInteractable
{
    IWorkerTask Claimer { get; }
    event Action<IWorkerInteractable> OnFinished;

    void OnDiscovered();
    bool TrySetClaimer(IWorkerTask claimer);
    void ClearClaimer(IWorkerTask claimer);
    Transform GetPosition();
}
