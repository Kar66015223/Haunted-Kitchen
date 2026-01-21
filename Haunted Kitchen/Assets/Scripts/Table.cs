using UnityEngine;

public class Table : MonoBehaviour, Iinteractable
{
    private Item currentItem;
    public Transform placePoint;

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem == null) return;

        if ((currentItem == null && playerItem.currentHeldItemObj != null))
        {
            PlaceItem(playerItem);
            return;
        }

        if ((currentItem != null && playerItem.currentHeldItemObj == null))
        {
            playerItem.PickUp(currentItem.itemData, currentItem.gameObject);
            currentItem = null;
            return;
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
}
