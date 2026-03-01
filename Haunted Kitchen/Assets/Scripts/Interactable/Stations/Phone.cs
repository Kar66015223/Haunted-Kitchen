using UnityEngine;

public class Phone : MonoBehaviour, Iinteractable
{
    [SerializeField] private ShopUI shopUI;

    public bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        if (interactor.source == null) 
            return false;

        return true;
    }

    public void Interact(Interactor interactor)
    {
        shopUI.Open();
    }
}
