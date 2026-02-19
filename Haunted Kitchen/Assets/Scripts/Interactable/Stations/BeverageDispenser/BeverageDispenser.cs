using UnityEngine;

public class BeverageDispenser : MonoBehaviour, Iinteractable, IContextInteractable
{
    [SerializeField] private ItemData refillItem;
    public GameObject beveragePrefab;
    public int beverageAmount = 0;
    [SerializeField] private int maxAmount;

    [SerializeField] private StationStatus status;

    private void Start()
    {
        beverageAmount = maxAmount;
    }

    public bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null) return false;

        if (playerItem.currentHeldItemObj == null && 
            status == StationStatus.Usable &&
            beverageAmount != 0)
            return true;

        if(playerItem.currentHeldItemObj != null &&
            beverageAmount == 0 &&
            IsCorrectRefillItem(playerItem))
            return true;

        return false;
    }

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem.currentHeldItemObj == null)
        {
            GiveItem(playerItem); 
        }
        else if (beverageAmount == 0 && IsCorrectRefillItem(playerItem))
        {
            RefillDispenser(playerItem);
        }
    }

    void GiveItem(PlayerItem playerItem)
    {
        GameObject prefab = Instantiate(beveragePrefab, transform.position, transform.rotation);

        Item itemPrefab = prefab.GetComponent<Item>();
        playerItem.PickUp(itemPrefab.itemData, prefab);

        beverageAmount--;   
    }

    bool IsCorrectRefillItem(PlayerItem playerItem)
    {
        Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
        if(heldItem == null) return false;

        return heldItem.itemData == refillItem;
    }

    public void RefillDispenser(PlayerItem playerItem)
    {
        beverageAmount = maxAmount;

        Destroy(playerItem.currentHeldItemObj);
        playerItem.DropItemNoRaycast();
    }
}
