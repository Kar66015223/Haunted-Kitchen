using System.IO;
using UnityEngine;

public abstract class Kitchenware : MonoBehaviour, Iinteractable
{
    public Transform cookPoint;

    private Item currentItem;
    protected float cookTimer = 0f;
    private bool isCooking;

    public KitchenwareStatus kitchenwareStatus;

    public enum KitchenwareStatus
    {
        Usable,
        Destroyed
    }

    public void Interact(GameObject interactor)
    {
        if (kitchenwareStatus == KitchenwareStatus.Usable)
        {
            PlayerItem playerItem = interactor.GetComponent<PlayerItem>();

            if (!isCooking && playerItem.currentHeldItemObj != null)
            {
                PlaceItem(playerItem);
            }
        }

        Debug.Log($"{gameObject.name} interacted with by {interactor.name}");
    }

    void PlaceItem(PlayerItem playerItem)
    {
        GameObject itemObj = playerItem.currentHeldItemObj;
        currentItem = itemObj.GetComponent<Item>();

        playerItem.DropItem();

        itemObj.transform.position = cookPoint.position;
        itemObj.transform.rotation = cookPoint.rotation;
        itemObj.transform.SetParent(transform);

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        StartCooking();
    }

    void StartCooking()
    {
        IngredientData ingredient = currentItem.itemData as IngredientData;

        if (ingredient == null)
        {
            Debug.Log("Item cannot be cooked");
            return;
        }

        isCooking = true;
    }

    void Update()
    {
        if (!isCooking) return;

        cookTimer -= Time.deltaTime;

        if (cookTimer <= 0f)
        {
            FinishCooking();
        }
    }

    void FinishCooking()
    {
        IngredientData ingredient = (IngredientData)currentItem.itemData;
        currentItem.itemData = ingredient.cookedResult;

        MeshRenderer itemMat = currentItem.GetComponent<MeshRenderer>();
        itemMat.material = ingredient.cookedMaterial;

        Debug.Log($"Cooked {ingredient.cookedResult.itemName}");

        isCooking = false;
    }
}
