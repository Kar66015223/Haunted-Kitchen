using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private List<ItemData> possibleOrders = new();
    [SerializeField] private CustomerOrderHabit orderHabit;

    [SerializeField] private List<ItemData> orderedItems = new();
    [SerializeField] private List<ItemData> servedItems = new();
    public int servedPrice;

    public event Action<bool, int> OnOrderServed;
    public event Action<List<ItemData>> OnOrderGenerated;
    public event Action<ItemData> OnItemServed;

    private void Awake()
    {
        orderHabit = GetComponent<CustomerOrderHabit>();

        if (orderHabit == null)
            Debug.LogError("No CustomerOrderHabit attached!");
    }

    public void GenerateOrder()
    {
        orderedItems = orderHabit.GenerateOrder(possibleOrders);

        servedItems.Clear();
        servedPrice = 0;

        OnOrderGenerated?.Invoke(orderedItems);
    }

    public void ServeOrder(PlayerItem playerItem)
    {
        ItemData served = playerItem.currentHeldItemData;

        servedItems.Add(served);
        servedPrice += served.price;

        GameObject servedObj = playerItem.currentHeldItemObj;
        playerItem.DropItemNoRaycast();
        Destroy(servedObj);

        OnItemServed?.Invoke(served);

        if (orderHabit.ShouldCheckOrder(servedItems, orderedItems))
        {
            CheckOrder();
        }

        //Debug.Log($"Served: {served.itemName}");
    }

    private void CheckOrder()
    {
        bool correct = orderHabit.ValidateOrder(servedItems, orderedItems);
        OnOrderServed?.Invoke(correct, servedPrice);
    }
}
