using UnityEngine;

public class Item : MonoBehaviour, Iinteractable
{
    public ItemData itemData;
    public ItemState itemState;

    public virtual bool CanInteract(Interactor interactor)
    {
        Debug.Log("Item.CanInteract Check");

        if (interactor == null)
        {
            Debug.Log("Interactor is null");
            return false;
        }

        if (interactor.interactionType == InteractionType.Hold)
        {
            Debug.Log("Interaction type is hold (not Press)");
            return false;
        }

        var playerItem = interactor.playerItem;
        if(playerItem == null)
        {
            Debug.Log("PlayerItem is null");
            return false;
        }

        if(itemState == ItemState.Held)
        {
            Debug.Log("ItemState is held");
            return false;
        }

        if (playerItem.currentHeldItemObj != null)
        {
            Debug.Log("Already holding something");
            return false;
        }

        return true;
    }

    public virtual void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;
        var currentTable = interactor.currentTable;

        if (playerItem == null) return;

        playerItem.PickUp(itemData, gameObject);
        currentTable?.SetItem(null);

        Debug.Log($"Interacted with item {itemData.itemName} by {interactor.source.name}");
    }
}
