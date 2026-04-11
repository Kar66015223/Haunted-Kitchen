using System.Collections.Generic;
using UnityEngine;

public class OrderDelivery
{
    public Customer_New customer;
    public List<ItemData> orderedItems;
    public List<Item> availableItems; //Items found in scene


    public bool IsValid
    {
        get
        {
            return customer != null &&
                customer.GetCurrentState() == CustomerState.Ordered ||
                HasItemsToPickUp;
        }
    }

    public bool HasItemsToPickUp => availableItems != null && availableItems.Count > 0;

    public float GetPatienceRemaining()
    {
        if (customer == null)
            return float.MaxValue;

        var patience = customer.GetComponent<CustomerPatience>();

        if (patience == null)
            return float.MaxValue;

        return patience.GetRemainingPatience();
    }
}
