using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private List<ItemData> possibleOrders = new();
    [SerializeField] private CustomerOrderHabit orderHabit;

    [SerializeField] private List<ItemData> orderedItems = new();
    [SerializeField] private List<ItemData> servedItems = new();
    private List<ItemData> inFlightItems = new(); // Items currently being fetched by a worker
    public int servedPrice;

    public List<ItemData> GetOrderedItems() => new List<ItemData>(orderedItems);

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

    public bool ReserveItem(ItemData data)
    {
        int orderedCount = orderedItems.Count(i => i == data);
        int servedCount = servedItems.Count(i => i == data);
        int reservedCount = inFlightItems.Count(i => i == data);

        if (reservedCount + servedCount < orderedCount)
        {
            inFlightItems.Add(data);
            return true;
        }

        return false;
    }

    public void CancelReserve(ItemData data)
    {
        inFlightItems.Remove(data);
    }

    public List<ItemData> GetRemainingNeededItems()
    {
        var remaining = new List<ItemData>(orderedItems);

        foreach (var item in servedItems)
            remaining.Remove(item);

        foreach (var item in inFlightItems)
            remaining.Remove(item);

        return remaining;
    }

    public void ServeOrderWorker(Item item)
    {
        ItemData served = item.itemData;

        servedItems.Add(served);
        servedPrice += served.price;

        OnItemServed?.Invoke(served);

        if (orderHabit.ShouldCheckOrder(servedItems, orderedItems))
        {
            CheckOrder();
        }
    }

    private void CheckOrder()
    {
        bool correct = orderHabit.ValidateOrder(servedItems, orderedItems);
        OnOrderServed?.Invoke(correct, servedPrice);
    }

    public bool ValidateWorkerOrder(List<ItemData> servedItems)
    {
        return orderHabit.ValidateOrder(servedItems, orderedItems);
    }
}
