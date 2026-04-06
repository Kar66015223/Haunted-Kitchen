using System.Collections.Generic;
using UnityEngine;

public class OrderDelivery : MonoBehaviour
{
    public Customer_New customer;
    public List<ItemData> orderedItems;
    public List<Item> availableItems; //Items found in scene

    public bool IsValid
    {
        get
        {
            if (customer == null || customer.GetCurrentState() != CustomerState.Ordered)
                return false;

            if (availableItems == null || availableItems.Count == 0)
                return false;

            return true;
        }
    }

    public float GetPatienceRemaining()
    {
        return customer ?
            GetComponent<CustomerPatience>().GetRemainingPatience() : float.MaxValue;
    }
}
