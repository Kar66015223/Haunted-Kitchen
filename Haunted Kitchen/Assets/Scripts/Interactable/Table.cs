using Unity.VisualScripting;
using UnityEngine;

public class Table : MonoBehaviour, Iinteractable, IContextInteractable
{
    [SerializeField] private Item currentItem;
    public Transform placePoint;

    public bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null)
            return false;

        // if table has MakingFood items & player is holding the right ingredient
        if (currentItem != null &&
            playerItem.currentHeldItemObj != null &&
            //currentItem.gameObject.TryGetComponent<ITableInteractable>(out _) &&
            currentItem.gameObject.TryGetComponent<IStationContextInteractable>(out var context))
        {
            return context.CanStationInteract(playerItem);
        }

        // if table has nothing & player has something (put item on table)
        if (currentItem == null && playerItem.currentHeldItemObj != null)
            return true;

        // if table has something & player has nothing (player take the object)
        if (currentItem != null && playerItem.currentHeldItemObj == null)
            return true;

        return false;
    }

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem == null) return;

        if (!CanInteract(playerItem))
        {
            return;
        }

        if ((currentItem == null && playerItem.currentHeldItemObj != null))
        {
            PlaceItem(playerItem);
            return;
        }

        if (currentItem == null) return;

        if (currentItem.TryGetComponent(out ITableInteractable tableInteractable))
        {
            if (tableInteractable.HandleTableInteraction(interactor))
                return;
        }

        if (playerItem.currentHeldItemObj == null)
        {
            playerItem.PickUp(currentItem.itemData, currentItem.gameObject);
            currentItem = null;
        }

        Debug.Log($"{gameObject.name} interacted with by {interactor.name}");
    }

    void PlaceItem(PlayerItem playerItem)
    {
        GameObject itemObj = playerItem.currentHeldItemObj;
        if (itemObj == null) return;

        currentItem = itemObj.GetComponent<Item>();
        if (currentItem == null) return;

        playerItem.DropItem();

        itemObj.transform.position = placePoint.position;
        itemObj.transform.rotation = placePoint.rotation;
        itemObj.transform.SetParent(transform);

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    public void SetItem(Item item)
    {
        currentItem = item;
    }
}
