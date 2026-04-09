using System.Collections.Generic;
using System.Linq;

public static class OrderMatcher
{
    public static OrderDelivery FindBestOrder(
        List<Customer_New> customers,
        List<Item> availableItems,
        Item currentTarget)
    {
        return customers
            .Select(customer => CreateDelivery(customer, availableItems, currentTarget))
            .Where(order => order != null && order.IsValid)
            .OrderBy(order => order.GetPatienceRemaining())
            .FirstOrDefault();
    }

    private static OrderDelivery CreateDelivery(
        Customer_New customer,
        List<Item> items,
        Item currentTarget)
    {
        var orderSystem = customer.GetComponent<CustomerOrder>();
        if (orderSystem == null) return null;

        var orderedData = orderSystem.GetOrderedItems();
        var matches = items.Where(item =>
            !item.IsTargeted || item == currentTarget)
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
