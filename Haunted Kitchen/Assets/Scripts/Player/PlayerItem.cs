using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public ItemData currentHeldItemData;
    public GameObject currentHeldItemObj;

    public void PickUp(ItemData data, GameObject itemObj)
    {
        currentHeldItemData = data;
        currentHeldItemObj = itemObj;

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        itemObj.tag = "Item";

        itemObj.transform.SetParent(transform);
        itemObj.transform.localPosition = new Vector3(0, 0, 1);
        itemObj.transform.localRotation = Quaternion.identity;

        Item item = itemObj.GetComponent<Item>();
        item.itemState = Item.ItemState.Held;
    }

    public void DropItem()
    {
        if (currentHeldItemObj == null) return;

        currentHeldItemObj.transform.SetParent(null, true);

        Item item = currentHeldItemObj.GetComponent<Item>();
        item.itemState = Item.ItemState.NotHeld;

        if (currentHeldItemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
        }

        currentHeldItemData = null;
        currentHeldItemObj = null;
    }
}
