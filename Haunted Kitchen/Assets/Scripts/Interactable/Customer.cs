using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Customer : MonoBehaviour, Iinteractable, IContextInteractable
{
    public List<ItemData> possibleOrders = new();
    public ItemData orderedItem;

    public Image idleUI;
    public Image orderUI;

    [SerializeField] private CustomerState state = CustomerState.Idle;
    
    public enum CustomerState
    {
        Idle,
        Ordered,
        Served
    }

    private void Start()
    {
        orderedItem = possibleOrders[Random.Range(0, possibleOrders.Count)];
        UpdateUI();
    }

    public bool CanInteract(PlayerItem playerItem)
    {
        switch (state)
        {
            case CustomerState.Idle:
                return true;

            case CustomerState.Ordered:
                if (playerItem == null) return false;
                if (playerItem.currentHeldItemObj == null) return false;

                return playerItem.currentHeldItemData is FoodData;

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
        UpdateUI();

        Debug.Log($"Customer ordered: {orderedItem}");
    }

    private void ServeFood(PlayerItem playerItem)
    {
        ItemData servedItem = playerItem.currentHeldItemData;
        bool correct = servedItem == orderedItem;

        if (correct)
        {
            Debug.Log("Correct order served!");
            // reward player
        }
        else
        {
            Debug.Log($"Wrong order! Expected {orderedItem.itemName}, got {servedItem.itemName}");
            // penalty / reaction
        }

        Destroy(playerItem.currentHeldItemObj);
        playerItem.DropItem();

        state = CustomerState.Served;

        UpdateUI();
    }

    public void UpdateUI()
    {
        idleUI.gameObject.SetActive(state == CustomerState.Idle);

        orderUI.gameObject.SetActive(state == CustomerState.Ordered);

        if (state == CustomerState.Ordered && orderedItem != null)
        {
            orderUI.sprite = orderedItem.icon;
        }
    }
}
