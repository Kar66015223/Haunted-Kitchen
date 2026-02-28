using UnityEngine;

public class Item : MonoBehaviour, Iinteractable, IContextInteractable
{
    public ItemData itemData;
    public ItemState itemState;

    public virtual bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null) 
            return false;

        if (itemState == ItemState.Held)
            return false;
        
        if (playerItem.currentHeldItemObj != null) 
            return false;

        return true;
    }

    public virtual void Interact(GameObject interactor)
    {
        Debug.Log($"Picking up {gameObject.name}");
    }
}
