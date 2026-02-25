using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NormalOrderHabit : CustomerOrderHabit
{
    [SerializeField] private int orderAmount = 1;

    public override List<ItemData> GenerateOrder(List<ItemData> allOrders)
    {
        List<ItemData> result = new();

        for (int i = 0; i < allOrders.Count; i++)
        {
            var item = allOrders[Random.Range(0, allOrders.Count)];
            result.Add(item);
        }

        return result;
    }

    public override bool ShouldCheckOrder(List<ItemData> served, List<ItemData> ordered)
    {
        if (served.Count > ordered.Count) return true;

        var orderedGrouped = ordered.GroupBy(i => i) //Group duplicated items together 
                                .ToDictionary(g => g.Key, g => g.Count()); //Turn it into "the item, how many times it appears"
        var servedGrouped = served.GroupBy(i => i)
                                  .ToDictionary(g => g.Key, g => g.Count());

        foreach (var kvp in servedGrouped) //kvp = KeyValuePair<ItemData, int>
        {
            if (!orderedGrouped.ContainsKey(kvp.Key))
                return true;
            if (kvp.Value > orderedGrouped[kvp.Key])
                return true;
        }

        if (served.Count == ordered.Count)
            return true;

        return false;
    }

    public override bool ValidateOrder(List<ItemData> served, List<ItemData> ordered)
    {
        // Group identical items together
        var orderedGrouped = ordered.GroupBy(i => i)
                                     .ToDictionary(g => g.Key, g => g.Count());

        // Group identical items together
        var servedGrouped = served.GroupBy(i => i)
                                        .ToDictionary(g => g.Key, g => g.Count());

        // Return true when orderedGrouped == servedGrouped.Count
        bool correct = orderedGrouped.Count == servedGrouped.Count &&
                       orderedGrouped.All(kvp =>
                           servedGrouped.ContainsKey(kvp.Key) &&
                           servedGrouped[kvp.Key] == kvp.Value);

        return correct;
    }
}
