using UnityEngine;

public abstract class Kitchenware : MonoBehaviour, Iinteractable, IContextInteractable
{
    public Transform cookPoint;

    private Item currentItem;
    protected float cookTimer = 0f;
    //protected abstract float CookTime { get; } <= uncomment if child class have their own cookTimer
    [SerializeField] private bool isCooking;

    public KitchenwareStatus kitchenwareStatus;

    public enum KitchenwareStatus
    {
        Usable,
        Destroyed
    }

    public bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null) return false;

        if (playerItem.currentHeldItemObj == null && currentItem != null && !isCooking)
            return true;

        if (playerItem.currentHeldItemObj != null)
        {
            Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            IngredientData ingredient = heldItem.itemData as IngredientData;

            if (ingredient == null || heldItem == null)
                return false;

            if (ingredient.isCookable)
                return true;
        }

        return false;
    }

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();

        if (playerItem == null) return;

        if (kitchenwareStatus == KitchenwareStatus.Usable && !isCooking && currentItem == null && playerItem.currentHeldItemObj != null)
        {
            Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            IngredientData ingredient = heldItem.itemData as IngredientData;

            if (ingredient == null || !ingredient.isCookable)
            {
                Debug.Log("This ingredient cannot be cooked");
                return;
            }

            PlaceItem(playerItem);
            return;
        }

        if (currentItem != null && !isCooking)
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

        cookTimer = ingredient.cookTime; //cookTimer = CookTime; <= use this instead if child class have their own cookTimer
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
        cookTimer = 0f;
    }
}
