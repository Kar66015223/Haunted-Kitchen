using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public ItemData currentHeldItemData;
    public GameObject currentHeldItemObj;

    public LayerMask floorLayer;
    [SerializeField] private LayerMask dropBlockLayers;
    public float dropRayDistance = 10f;
    public float dropHeightOffset = 0.5f;

    private PlayerAnimation anim;

    [SerializeField] private Transform holdPointOneHand;

    [SerializeField] private Transform holdPointTwoHand;

    void Awake()
    {
        anim = GetComponent<PlayerAnimation>();
    }

    private void Update()
    {
        bool isHolding = currentHeldItemData != null;

        if (!isHolding)
        {
            anim.SetHoldOneHand(false);
            anim.SetHoldTwoHand(false);
            return;
        }

        if (currentHeldItemData.oneHand)
        {
            anim.SetHoldOneHand(true);
            return;
        }

        anim.SetHoldTwoHand(true);
    }

    public void PickUp(ItemData data, GameObject itemObj)
    {
        if (data == null || itemObj == null)
        {
            Debug.LogWarning("PickUp called with null data or itemObj");
            return;
        }

        // If we're already holding something, refuse (caller should check)
        if (currentHeldItemData != null)
        {
            Debug.LogWarning("PickUp called while already holding an item");
            return;
        }

        currentHeldItemData = data;
        currentHeldItemObj = itemObj;

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        var colliders = itemObj.GetComponentsInChildren<Collider>();
        foreach (var c in colliders)
        {
            c.enabled = false;
        }

        if (currentHeldItemData.oneHand)
        {
            itemObj.transform.SetParent(holdPointOneHand, true);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
        }

        if (!currentHeldItemData.oneHand)
        {
            itemObj.transform.SetParent(holdPointTwoHand, true);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
        }

        if (itemObj.TryGetComponent(out Item item))
        {
            item.itemState = ItemState.Held;
        }
    }

    public void DropItem()
    {
        if (currentHeldItemObj == null) return;

        if(currentHeldItemData != null && !currentHeldItemData.canDrop) return;

        Transform itemTransform = currentHeldItemObj.transform;

        #region Raycast
        Vector3 rayOrigin = itemTransform.position + Vector3.up * dropHeightOffset;

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
        if (col == null) return;

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
        #endregion

        var colliders = currentHeldItemObj.GetComponentsInChildren<Collider>();
        foreach (var c in colliders)
        {
            c.enabled = true;
        }

        currentHeldItemObj.transform.SetParent(null);
        itemTransform.position = dropPosition;
        itemTransform.rotation = Quaternion.identity;

        if (currentHeldItemObj.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = false;

        var itemComp = currentHeldItemObj.GetComponent<Item>();
        if (itemComp != null)
        {
            itemComp.itemState = ItemState.NotHeld;
        }

        currentHeldItemData = null;
        currentHeldItemObj = null;

        anim.SetHoldOneHand(false);
        anim.SetHoldTwoHand(false);
    }

    public void DropItemNoRaycast()
    {
        if (currentHeldItemData == null) return;
        if (currentHeldItemObj == null) return;

        var colliders = currentHeldItemObj.GetComponentsInChildren<Collider>();
        foreach (var c in colliders)
        {
            c.enabled = true;
        }

        if (currentHeldItemObj.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = false;

        if (currentHeldItemObj.TryGetComponent(out Item itm))
            itm.itemState = ItemState.NotHeld;

        currentHeldItemObj.transform.SetParent(null, true); 
        currentHeldItemData = null; 
        currentHeldItemObj = null;

        anim.SetHoldOneHand(false);
        anim.SetHoldTwoHand(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
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
#endif
}
