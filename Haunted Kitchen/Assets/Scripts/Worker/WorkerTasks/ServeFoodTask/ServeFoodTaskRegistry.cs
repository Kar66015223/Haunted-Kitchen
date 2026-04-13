using System.Collections.Generic;
using UnityEngine;

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

            // Debug.Log($"Found {customer.gameObject.name}, Discovered customers: {Customers.Count}");
        }
        else if (target is Item item &&
        item.itemData is FoodData &&
        !Items.Contains(item))
        {
            Items.Add(item);
            item.OnFinished += _ => Items.Remove(item);

            // Debug.Log($"Found {item.gameObject.name}, Discovered items: {Items.Count}");
        }
    }
    
    public void Cleanup()
    {
        // Customers.RemoveAll(c => c == null || c.GetCurrentState() != CustomerState.Ordered);
        // Items.RemoveAll(i => i == null || i.GetItemState() == ItemState.Held);
        Customers.RemoveAll(c => c == null);
        Items.RemoveAll(i => i == null);
    }
}
