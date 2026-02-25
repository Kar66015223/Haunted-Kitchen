using UnityEngine;
using System.Collections.Generic;

public abstract class CustomerOrderHabit : MonoBehaviour
{
    public abstract List<ItemData> GenerateOrder(List<ItemData> allOrders);
    
    public abstract bool ValidateOrder(List<ItemData> served, List<ItemData> ordered);

    public abstract bool ShouldCheckOrder(List<ItemData> served, List<ItemData> ordered);
}
