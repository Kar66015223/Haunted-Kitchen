using Unity.VisualScripting;
using UnityEngine;

public class Table : MonoBehaviour, Iinteractable, IContextInteractable, IHoldForwarder
{
    [SerializeField] private Item currentItem;
    public Transform placePoint;
    public Transform customerStandPoint;

    public bool isOccupied;

    public bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null)
            return false;

        if (customerStandPoint != null)
            return false;

        // Support hold interaction
        if (HasHoldable())
        {
            return true;
        }

        // Station + Container
        if (currentItem != null &&
            currentItem.TryGetComponent(out IStationContextInteractable station) &&
            playerItem.currentHeldItemObj != null &&
            playerItem.currentHeldItemObj.TryGetComponent(out ContainerItem container))
        {
            return station.CanContainerInteract(container);
        }

        // Station + Ingredient item
        if (currentItem != null &&
            playerItem.currentHeldItemObj != null &&
            currentItem.TryGetComponent(out IStationContextInteractable context))
        {
            return context.CanStationInteract(playerItem);
        }

        // put item on table
        if (currentItem == null && playerItem.currentHeldItemObj != null)
            return true;

        // take item from table
        if (currentItem != null && playerItem.currentHeldItemObj == null)
            return true;

        return false;
    }

    public void Interact(GameObject interactor)
    {
        // If item supports hold, do not treat it like normal item
        if (currentItem != null && currentItem is IHoldInteractable)
        {
            currentItem.Interact(interactor);
            return;
        }

        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem == null) return;

        if (!CanInteract(playerItem))
        {
            return;
        }

        //if currentItem is a MakingFood item & player is holding Container item
        if (currentItem != null &&
            currentItem.TryGetComponent(out IStationContextInteractable station) &&
            playerItem.currentHeldItemObj != null &&
            playerItem.currentHeldItemObj.TryGetComponent(out ContainerItem container))
        {
            if (station.CanContainerInteract(container))
            {
                station.HandleContainer(container, this);
                return;
            }
        }

        if (currentItem == null && playerItem.currentHeldItemObj != null)
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

    public void ForwardHold(GameObject interactor)
    {
        if (currentItem != null &&
            currentItem is IHoldInteractable hold)
        {
            hold.HoldInteract(interactor);
        }
    }

    public bool HasHoldable()
    {
        return currentItem != null && currentItem is IHoldInteractable;
    }

    void PlaceItem(PlayerItem playerItem)
    {
        GameObject itemObj = playerItem.currentHeldItemObj;
        if (itemObj == null) return;

        currentItem = itemObj.GetComponent<Item>();
        if (currentItem == null) return;

        playerItem.DropItemNoRaycast();
        currentItem.itemState = ItemState.NotHeld;

        itemObj.transform.position = placePoint.position;
        itemObj.transform.rotation = placePoint.rotation;
        itemObj.transform.SetParent(transform, true);

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }
    }

    public void SetItem(Item item)
    {
        currentItem = item;
    }

    public void ClearTableOccupy()
    {
        isOccupied = false;
    }
}
