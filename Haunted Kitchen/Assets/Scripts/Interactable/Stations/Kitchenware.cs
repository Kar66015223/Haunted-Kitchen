using Unity.VisualScripting;
using UnityEngine;

public class Kitchenware : MonoBehaviour, Iinteractable, IDestroyable
{
    public Transform cookPoint;

    private Item currentItem;
    private float cookTimer = 0f;
    [SerializeField] private bool isCooking;

    [SerializeField] private GameObject DestroyedVFX;

    [SerializeField] private StationStatus status;
    [SerializeField] private CookingMethod supportedMethod;

    public bool CanInteract(Interactor interactor)
    {
        if(interactor == null || status == StationStatus.Destroyed) 
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        var playerItem = interactor.playerItem;

        if(playerItem == null) 
            return false;

        //If currentItem is not cooking, allow pickup
        if (playerItem.currentHeldItemObj == null &&
            currentItem != null &&
            !isCooking)
            return true;

        //If currentItem == null & player has something that is ingredient and matched CookingMethod, allow placing
        if (playerItem.currentHeldItemObj != null &&
            currentItem == null)
        {
            var heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            var ingredient = heldItem?.itemData as IngredientData;

            if(ingredient == null)
                return false;
            if(!ingredient.isCookable)
                return false;
            if(ingredient.method != supportedMethod)
                return false;

            return true;
        }

        return false;
    }

    public void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;
        if(playerItem == null) return;

        //If usable & not cooking & empty & player has something, place
        if (status == StationStatus.Usable &&
            !isCooking &&
            currentItem == null &&
            playerItem.currentHeldItemObj != null)
        {
            var heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            var ingredient = heldItem?.itemData as IngredientData;

            if (ingredient == null)
            {
                Debug.Log("This ingredient can't be cooked");
                return;
            }
            if (ingredient.method != supportedMethod)
            {
                Debug.Log("This kitchemware can't cook this ingredient");
                return;
            }

            PlaceItem(playerItem);
            return;
        }

        //If not empty and not cooking, pickup
        if (currentItem != null && 
            !isCooking)
        {
            playerItem.PickUp(currentItem.itemData, currentItem.gameObject);
            currentItem = null;
        }

        Debug.Log($"{gameObject.name} interacted with by {interactor.source.name}");
    }

    void PlaceItem(PlayerItem playerItem)
    {
        GameObject itemObj = playerItem.currentHeldItemObj;
        if (itemObj == null) return;

        currentItem = itemObj.GetComponent<Item>();
        if (currentItem == null) return;

        playerItem.DropItemNoRaycast();

        itemObj.transform.position = cookPoint.position;
        itemObj.transform.rotation = cookPoint.rotation;
        itemObj.transform.SetParent(cookPoint, true);

        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
        }

        StartCooking();
    }

    void StartCooking()
    {
        var ingredient = currentItem.itemData as IngredientData;

        if (ingredient == null)
        {
            Debug.Log("Item cannot be cooked");
            return;
        }

        cookTimer = ingredient.cookTime;
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
        var ingredient = (IngredientData)currentItem.itemData;
        currentItem.itemData = ingredient.cookedResult;

        if (ingredient.cookedModel != null)
        {
            GameObject cookedObj = Instantiate(ingredient.cookedModel,
            currentItem.transform.position,
            currentItem.transform.rotation);

            cookedObj.transform.SetParent(currentItem.transform.parent, true);

            Destroy(currentItem.gameObject);
            currentItem = cookedObj.GetComponent<Item>();
        }

        else
        {
            MeshRenderer[] itemMat = currentItem.GetComponentsInChildren<MeshRenderer>();
            foreach (var r in itemMat)
            {
                r.material = ingredient.cookedMaterial;
            }
        }

        Debug.Log($"Cooked {ingredient.cookedResult.itemName}");

        isCooking = false;
        cookTimer = 0f;
    }

    public void SetStationStatus(StationStatus newStatus)
    {
        status = newStatus;
        DestroyedVFX?.SetActive(status == StationStatus.Destroyed);
    }
}
