using UnityEngine;

public class BoxItem : Item, IHoldInteractable
{
    [SerializeField] private BoxData data;
    [SerializeField] private int currentAmount = 0;

    private void Start()
    {
        currentAmount = data.maxAmount;
    }

    public override bool CanInteract(PlayerItem playerItem)
    {
        if (!base.CanInteract(playerItem))
            return false;

        return currentAmount > 0;
    }

    public override void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem == null) return;

        TakeItem(playerItem);
    }

    public void HoldInteract(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();
        if (playerItem.currentHeldItemObj == null)
        {
            playerItem.PickUp(data, gameObject);
        }
    }

    private void TakeItem(PlayerItem playerItem)
    {
        GameObject prefab = Instantiate(data.content, transform.position, transform.rotation);

        Item itemPrefab = prefab.GetComponent<Item>();
        playerItem.PickUp(itemPrefab.itemData, prefab);

        currentAmount--;
    }
}
