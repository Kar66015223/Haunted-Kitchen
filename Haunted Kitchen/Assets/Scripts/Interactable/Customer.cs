using UnityEngine;
using System.Collections.Generic;

public class Customer : MonoBehaviour, Iinteractable, IContextInteractable
{
    public List<ItemData> possibleOrders = new();
    public ItemData orderedItem;

    private CustomerState state = CustomerState.Idle;
    
    public enum CustomerState
    {
        Idle,
        Ordered,
        Served
    }

    private void Start()
    {
        orderedItem = possibleOrders[Random.Range(0, possibleOrders.Count)];
    }

    public bool CanInteract(PlayerItem playerItem)
    {
        switch (state)
        {
            case CustomerState.Idle:
                return true;

            case CustomerState.Ordered:
                if (playerItem == null) return false;
                if(playerItem.currentHeldItemObj == null) return false;

                return playerItem.currentHeldItemData == orderedItem;

            case CustomerState.Served:
                return false;

        }

        return false;
    }

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();

        switch (state)
        {
            case CustomerState.Idle:
                TakeOrder();
                break;

            case CustomerState.Ordered:
                ServeFood(playerItem);
                break;
        }
    }

    private void TakeOrder()
    {
        state = CustomerState.Ordered;

        Debug.Log($"Customer ordered: {orderedItem}");
    }

    private void ServeFood(PlayerItem playerItem)
    {
        Debug.Log($"Served");

        Destroy(playerItem.currentHeldItemObj);
        playerItem.DropItem();

        state = CustomerState.Served;
    }
}
