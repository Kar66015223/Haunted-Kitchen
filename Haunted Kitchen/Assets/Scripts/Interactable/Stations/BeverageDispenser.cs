using Unity.VisualScripting;
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

    [SerializeField] private GameObject destroyedVFX;

    [SerializeField] private StationStatus status;
    public StationStatus Status => status;

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip getItemSound;

    [SerializeField] private AudioClip destroySound;
    [SerializeField] private AudioClip repairSound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        beverageAmount = maxAmount;
        UpdateUI();
    }

    public bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        var playerItem = interactor.playerItem;

        if (playerItem == null) return false;

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

        if (status == StationStatus.Destroyed && playerItem.currentHeldItemData != null)
        {
            Item heldItem = playerItem.currentHeldItemObj.GetComponent<Item>();
            RepairKit repairKit = heldItem.GetComponent<RepairKit>();

            if (repairKit != null)
            {
                repairKit.Repair(this);
                return;
            }
        }

        if (playerItem.currentHeldItemObj == null)
        {
            GiveItem(playerItem);
        }
        else if (beverageAmount == 0 && IsCorrectRefillItem(playerItem))
        {
            RefillDispenser(playerItem);
        }
    }

    void Update()
    {
        if (destroyedVFX != null)
        {
            destroyedVFX.SetActive(status == StationStatus.Destroyed);
        }
    }

    void GiveItem(PlayerItem playerItem)
    {
        GameObject prefab = Instantiate(beveragePrefab, transform.position, transform.rotation);

        Item itemPrefab = prefab.GetComponent<Item>();
        playerItem.PickUp(itemPrefab.itemData, prefab);

        beverageAmount--;

        UpdateUI();

        PlaySound(getItemSound);
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

        if (status == StationStatus.Destroyed)
            PlaySound(destroySound);

        if (status == StationStatus.Usable)
            PlaySound(repairSound);
    }

    public void PlaySound(AudioClip clip)
    {
        if (source.isPlaying)
            source.Stop();

        source.clip = clip;
        source.Play();
    }
}
