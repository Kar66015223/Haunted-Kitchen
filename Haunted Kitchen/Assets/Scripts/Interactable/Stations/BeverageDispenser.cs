using UnityEngine;

public class BeverageDispenser : MonoBehaviour, Iinteractable, IContextInteractable
{
    public GameObject beveragePrefab;
    public int beverageAmount = 0;

    [SerializeField] private StationStatus status;

    public bool CanInteract(PlayerItem playerItem)
    {
        if (playerItem == null) return false;

        if (playerItem.currentHeldItemObj == null && 
            status == StationStatus.Usable &&
            beverageAmount != 0)
            return true;

        return false;
    }

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        GiveItem(playerItem);
    }

    void GiveItem(PlayerItem playerItem)
    {
        GameObject prefab = Instantiate(beveragePrefab, transform.position, transform.rotation);

        Item itemPrefab = prefab.GetComponent<Item>();
        playerItem.PickUp(itemPrefab.itemData, prefab);

        beverageAmount--;   
    }
}
