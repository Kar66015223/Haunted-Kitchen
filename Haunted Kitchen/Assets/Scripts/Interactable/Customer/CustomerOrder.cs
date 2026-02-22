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

        Debug.Log($"Served: {served.itemName}");

        //Update order UI
    }

    private void CheckOrder()
    {
        bool correct = orderedItems.Count == servedItems.Count &&
                   !orderedItems.Except(servedItems).Any();

        OnOrderServed?.Invoke(correct, servedPrice);
    }
}
