using UnityEngine;
using System.Collections.Generic;

// Discovery & Cleanup logic
public class ServeFoodTaskRegistry
{
    public List<Customer_New> Customers { get; } = new();
    public List<Item> Items { get; } = new();

    public void Register(IWorkerInteractable target)
    {
        if (target is Customer_New customer &&
        !Customers.Contains(customer))
        {
            Customers.Add(customer);
            customer.OnFinished += _ => Customers.Remove(customer);
        }
        else if (target is Item item &&
        item.itemData is FoodData &&
        !Items.Contains(item))
        {
            Items.Add(item);
            item.OnFinished += _ => Items.Remove(item);
        }
    }
    
    public void Cleanup()
    {
        Customers.RemoveAll(c => c == null || c.GetCurrentState() != CustomerState.Ordered);
        Items.RemoveAll(i => i == null || i.GetItemState() == ItemState.Held);
    }
}
