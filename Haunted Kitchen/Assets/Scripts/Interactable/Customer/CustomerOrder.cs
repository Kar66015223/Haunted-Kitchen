using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerOrder : MonoBehaviour
{
    [SerializeField] private int orderAmount = 1;
    [SerializeField] private List<ItemData> possibleOrders = new();

    public List<ItemData> orderedItems = new();
    [SerializeField] private List<ItemData> servedItems = new();
    public int servedPrice;

    public event Action<bool, int> OnOrderServed;
    public event Action<List<ItemData>> OnOrderGenerated;
    public event Action<ItemData> OnItemServed;

    public void GenerateOrder()
    {
        if (possibleOrders.Count == 0)
        {
            Debug.LogError("Customer has no possible orders!");
            return;
        }

        orderedItems.Clear();
        servedItems.Clear();
        servedPrice = 0;

        for (int i = 0; i < orderAmount; i++)
        {
            ItemData item = possibleOrders[UnityEngine.Random.Range(0, possibleOrders.Count)];
            orderedItems.Add(item);
        }

        foreach (ItemData item in orderedItems)
        {
            Debug.Log($"{gameObject.name} ordered {item.itemName}");
        }

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

        if (servedItems.Count >= orderedItems.Count)
        {
            CheckOrder();
        }

        OnItemServed?.Invoke(served);

        Debug.Log($"Served: {served.itemName}");
    }

    private void CheckOrder()
    {
        // Group identical items together
        var orderedGrouped = orderedItems.GroupBy(i => i)
                                     .ToDictionary(g => g.Key, g => g.Count());

        // Group identical items together
        var servedGrouped = servedItems.GroupBy(i => i)
                                        .ToDictionary(g => g.Key, g => g.Count());

        // Return true when orderedGrouped == servedGrouped.Count
        bool correct = orderedGrouped.Count == servedGrouped.Count &&
                       orderedGrouped.All(kvp =>
                           servedGrouped.ContainsKey(kvp.Key) &&
                           servedGrouped[kvp.Key] == kvp.Value);

        OnOrderServed?.Invoke(correct, servedPrice);
    }
}
