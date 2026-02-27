using System.Collections.Generic;
using UnityEngine;

public class TrollOrderHabit : CustomerOrderHabit
{
    [SerializeField] private int orderAmount = 2;
    [SerializeField] private ItemData correctItem;

    public override List<ItemData> GenerateOrder(List<ItemData> allOrders)
    {
        List<ItemData> copy = new List<ItemData>(allOrders); //A copy of allOrders
        List<ItemData> result = new();

        orderAmount = Mathf.Min(orderAmount, copy.Count);

        for (int i = 0; i < orderAmount; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index); //Remove added items to prevent duplicates
        }

        correctItem = result[Random.Range(0, result.Count)];

        return result;
    }

    public override bool ShouldCheckOrder(List<ItemData> served, List<ItemData> ordered)
    {
        return served != null && served.Count > 0;
    }

    public override bool ValidateOrder(List<ItemData> served, List<ItemData> ordered)
    {
        if(served == null || served.Count == 0) 
            return false;

        foreach (var item in served)
        {
            if (item == correctItem)
                return true;
            else
                return false;
        }

        return false;
    }
}
