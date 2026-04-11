using System.Collections.Generic;
using System.Linq;

// Decides which customer is the priority and find their order
public static class OrderMatcher
{
    public static OrderDelivery FindBestOrder(
        List<Customer_New> customers,
        List<Item> availableItems,
        Item currentItemTarget,
        Customer_New currentCustomerTarget)
    {
        return customers
            .Select(customer => CreateDelivery(
                customer, availableItems, currentItemTarget, currentCustomerTarget))
            .Where(order => order != null && order.IsValid)
            .OrderBy(order => order.GetPatienceRemaining())
            .FirstOrDefault();
    }

    private static OrderDelivery CreateDelivery(
        Customer_New customer,
        List<Item> items,
        Item currentItemTarget,
        Customer_New currentCustomerTarget)
    {
        // if (customer.IsTargeted && customer != currentCustomerTarget)
        //     return null;
        if (customer.GetCurrentState() != CustomerState.Ordered)
            return null;
            
        var orderSystem = customer.GetComponent<CustomerOrder>();
        if (orderSystem == null) return null;

        var orderedData = orderSystem.GetOrderedItems();

        var matches = items
            .Where(item =>
            {
                bool isAvailable = !item.IsTargeted || item == currentItemTarget;
                bool isNotHeld = item.GetItemState() != ItemState.Held;

                return isAvailable && isNotHeld;
            })
            .Where(item => orderedData.Any(data => data.itemName == item.itemData.itemName))
            .ToList();

        if (matches.Count == 0) return null;

        return new OrderDelivery
        {
            customer = customer,
            orderedItems = orderedData,
            availableItems = matches
        };
    }
}
