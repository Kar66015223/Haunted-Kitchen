using UnityEngine;

public class Kitchenware : MonoBehaviour, Iinteractable, IDestroyable
{
    public Transform cookPoint;

    private Item currentItem;
    public Item CurrentItem => currentItem;
    [SerializeField] private GameObject itemVisual;

    private float cookTimer = 0f;
    [SerializeField] private bool isCooking;
    public bool IsCooking => isCooking;

    [SerializeField] private GameObject destroyedVFX;

    [SerializeField] private StationStatus status;
    public StationStatus Status => status;
    [SerializeField] private CookingMethod supportedMethod;

    [SerializeField] private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        var playerItem = interactor.playerItem;

        if (playerItem == null)
            return false;

        //If is broken and holding repair kit, allow fixing
        if (status == StationStatus.Destroyed && playerItem.currentHeldItemData != null)
        {
            Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            RepairKit repairKit = heldItem.GetComponent<RepairKit>();

            if (repairKit != null)
            {
                return true;
            }

            return false;
        }

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

            if (ingredient == null)
                return false;
            if (!ingredient.isCookable)
                return false;
            if (ingredient.method != supportedMethod)
                return false;

            return true;
        }

        return false;
    }

    public void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;
        if (playerItem == null) return;
        
        if(status == StationStatus.Destroyed && playerItem.currentHeldItemData != null)
        {
            Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            RepairKit repairKit = heldItem.GetComponent<RepairKit>();

            if (repairKit != null)
            {
                repairKit.Repair(this);
                return;
            }
        }

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
        status = StationStatus.CantDestroy;
    }

    void Update()
    {
        if (destroyedVFX != null)
        {
            destroyedVFX.SetActive(status == StationStatus.Destroyed);
        }
        
        if (!isCooking) return;

        cookTimer -= Time.deltaTime;

        if (!source.isPlaying)
            source.Play();

        if (cookTimer <= 0f)
        {
            FinishCooking();
            source.Stop();
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
            MeshRenderer[] itemMat = currentItem.GetComponentsInChildren<MeshRenderer>(true);
            foreach (var r in itemMat)
            {
                r.material = ingredient.cookedMaterial;
            }
        }

        Debug.Log($"Cooked {ingredient.cookedResult.itemName}");

        isCooking = false;
        cookTimer = 0f;
        status = StationStatus.Usable;
    }

    public void SetStationStatus(StationStatus newStatus)
    {
        status = newStatus;
    }

    public void ToggleItemVisual(bool value)
    {
        if (currentItem != null)
        {
            if (itemVisual == null)
            {
                itemVisual = currentItem.GetComponentInChildren<Outline>().gameObject;
            }

            if (itemVisual != null)
            {
                itemVisual.SetActive(value);
                Debug.Log($"Setting visual to {value}");
            }
        }
    }

    public void ClearItemVisual()
    {
        itemVisual = null;
    }
}
