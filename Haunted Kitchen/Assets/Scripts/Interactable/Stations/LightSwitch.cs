using UnityEngine;

public class LightSwitch : MonoBehaviour, Iinteractable
{
    public bool CanInteract(Interactor interactor)
    {
        if(interactor == null) 
            return false;

        if(interactor.interactionType == InteractionType.Hold)
            return false;

        return true;
    }

    public void Interact(Interactor interactor)
    {
        Debug.Log("interacting with LightSwitch");
    }
}
