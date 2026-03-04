using UnityEngine;
using UnityEngine.UI;

public class BeverageDispenser : MonoBehaviour, Iinteractable, IDestroyable
{
    [SerializeField] private ItemData refillItem;
    public GameObject beveragePrefab;
    public int beverageAmount = 0;
    [SerializeField] private int maxAmount;

    [Header("UI")]
    [SerializeField] private Image amountImg;

    [SerializeField] private GameObject DestroyedVFX;

    [SerializeField] private StationStatus status;

    private void Start()
    {
        beverageAmount = maxAmount;

        UpdateUI();
    }

    public bool CanInteract(Interactor interactor)
    {
        if (interactor == null || status == StationStatus.Destroyed)
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        var playerItem = interactor.playerItem;

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

    public void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;
        if (playerItem == null) return;

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

        UpdateUI();
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

        UpdateUI();
    }

    void UpdateUI()
    {
        float normalized = beverageAmount / (float)maxAmount;
        amountImg.fillAmount = normalized;
    }

    public void SetStationStatus(StationStatus newStatus)
    {
        status = newStatus;
        DestroyedVFX?.SetActive(status == StationStatus.Destroyed);
    }
}
