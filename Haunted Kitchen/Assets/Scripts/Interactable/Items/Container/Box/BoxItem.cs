using UnityEngine;

public class BoxItem : Item, Iinteractable
{
    [SerializeField] private BoxData data;
    [SerializeField] private int currentAmount = 0;

    private void Start()
    {
        data = itemData as BoxData;
        currentAmount = data.maxAmount;
    }

    public override bool CanInteract(Interactor interactor)
    {
        if(interactor == null) 
            return false;
        var playerItem = interactor.playerItem;

        //Allow hold interaction to pick up the box (if hands free)
        if (interactor.interactionType == InteractionType.Hold)
        {
            if(playerItem == null) 
                return false;
            if (playerItem.currentHeldItemData != null)
                return false;
            if (itemState == ItemState.Held)
                return false;

            return true;
        }

        //Allow normal interaction to take content (if hands free)
        if (interactor.interactionType == InteractionType.Press)
        {
            if(playerItem == null)
                return false;
            if(playerItem.currentHeldItemData != null)
                return false;
            if(currentAmount <= 0)
                return false;

            return true;
        }

        return false;
    }

    public override void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;

        if (interactor.interactionType == InteractionType.Hold)
        {
            if (playerItem != null)
            {
                playerItem.PickUp(itemData, gameObject);
                interactor.currentTable?.SetItem(null);
                Debug.Log($"Picked up {itemData.itemName}");
            }

            return;
        }

        if (interactor.interactionType == InteractionType.Press)
        {
            if (currentAmount <= 0)
            {
                Debug.Log($"{itemData.itemName} is empty");
                return;
            }

            if (data.content == null)
            {
                Debug.LogError("Box content prefab is missing in BoxData");
                return;
            }

            TakeItem(playerItem);
        }
    }

    private void TakeItem(PlayerItem playerItem)
    {
        GameObject prefab = Instantiate(data.content, transform.position, transform.rotation);

        Item itemPrefab = prefab.GetComponent<Item>();
        if (itemPrefab != null && playerItem != null)
        {
            playerItem.PickUp(itemPrefab.itemData, prefab);
        }
        else
        {
            Debug.LogWarning("Spawned content does not have Item component or no player to pick it up");
        }

        currentAmount--;
    }
}
