using UnityEngine;

public class Item : MonoBehaviour, Iinteractable
{
    public ItemData itemData;
    public ItemState itemState;

    public virtual bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
        {
            return false;
        }

        if (interactor.interactionType == InteractionType.Hold)
        {
            return false;
        }

        var playerItem = interactor.playerItem;
        if(playerItem == null)
        {
            return false;
        }

        if(itemState == ItemState.Held)
        {
            return false;
        }

        if (playerItem.currentHeldItemObj != null)
        {
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
