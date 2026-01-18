using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEngine.Rendering.DynamicArray<T>;

public class PlayerItem : MonoBehaviour
{
    public ItemData currentHeldItemData;
    public GameObject currentHeldItemObj;
    public float rayLength = 10f;

    private void Update()
    {
        if (currentHeldItemData != null)
        {
            Debug.Log(currentHeldItemData.name); 
        }
    }

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
    }

    public void DropItem()
    {
        if (currentHeldItemObj == null) return;

        currentHeldItemObj.transform.SetParent(null, true);

        //Vector3 rayOrigin = currentHeldItemObj.transform.position + Vector3.up;
        //Ray ray = new Ray(rayOrigin, Vector3.down);

        //int groundLayerMask = LayerMask.GetMask("Ground");
        //if (Physics.Raycast(ray, out RaycastHit hit, rayLength, groundLayerMask))
        //{
        //    currentHeldItemObj.transform.position = hit.point + Vector3.up * 0.5f;
        //}

        if (currentHeldItemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = false;
        }

        currentHeldItemData = null;
        currentHeldItemObj = null;
    }
}
