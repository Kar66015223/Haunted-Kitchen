using UnityEngine;

public interface Iinteractable
{
    bool CanInteract(Interactor interactor);
    void Interact(Interactor interactor);
}