using System.Collections.Generic;

public class OrderDelivery
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
        if (customer == null)
            return float.MaxValue;

        var patience = customer.GetComponent<CustomerPatience>();

        if (patience == null)
            return float.MaxValue;

        return patience.GetRemainingPatience();
    }
}
