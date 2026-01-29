using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public ItemData currentHeldItemData;
    public GameObject currentHeldItemObj;

    public LayerMask floorLayer;
    [SerializeField] private LayerMask dropBlockLayers;
    public float dropRayDistance = 10f;
    public float dropHeightOffset = 0.5f;

    public void PickUp(ItemData data, GameObject itemObj)
    {
        currentHeldItemData = data;
        currentHeldItemObj = itemObj;

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        itemObj.transform.SetParent(transform);
        itemObj.transform.localPosition = new Vector3(0, 0, 1);
        itemObj.transform.localRotation = Quaternion.identity;

        Item item = itemObj.GetComponent<Item>();
        item.itemState = Item.ItemState.Held;
    }

    public void DropItem()
    {
        if (currentHeldItemObj == null) return;

        Transform itemTransform = currentHeldItemObj.transform;

        Vector3 rayOrigin = itemTransform.position + Vector3.up * dropHeightOffset;

        //Raycast down 
        if (!Physics.Raycast(
            rayOrigin,
            Vector3.down,
            out RaycastHit hit,
            dropRayDistance,
            floorLayer))
        {
            Debug.Log("No floor detected, cannot drop item");
            return;
        }

        Collider col = currentHeldItemObj.GetComponent<Collider>();
        if(col ==  null) return;
        Vector3 halfExtents = col.bounds.extents;
        Vector3 dropPosition = hit.point + Vector3.up * halfExtents.y;

        bool blocked = Physics.CheckBox(
            dropPosition,
            halfExtents,
            itemTransform.rotation,
            dropBlockLayers,
            QueryTriggerInteraction.Ignore);

        if (blocked)
        {
            Debug.Log("Can't drop here");
            return;
        }

        // Perform drop
        currentHeldItemObj.transform.SetParent(null);

        itemTransform.position = dropPosition;
        itemTransform.rotation = Quaternion.identity;

        if (currentHeldItemObj.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = false;

        currentHeldItemObj.GetComponent<Item>().itemState = Item.ItemState.NotHeld;

        currentHeldItemData = null;
        currentHeldItemObj = null;
    }

    private void OnDrawGizmos()
    {
        if (currentHeldItemObj == null) return;

        Transform itemTransform = currentHeldItemObj.transform;
        Vector3 rayOrigin = itemTransform.position + Vector3.up * dropHeightOffset;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * dropRayDistance);

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, dropRayDistance, floorLayer))
        {
            Collider col = currentHeldItemObj.GetComponent<Collider>();
            if (col == null) return;

            Vector3 halfExtents = col.bounds.extents;
            Vector3 dropPosition = hit.point + Vector3.up * halfExtents.y;

            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(dropPosition, itemTransform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
        }
    }
}
