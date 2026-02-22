using System.Collections.Generic;
using UnityEngine;

public abstract class CustomerOrderStrategy : MonoBehaviour
{
    public abstract List<ItemData> GenerateOrder(List<ItemData> possibleOrders);
}
