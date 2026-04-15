using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanelUI : MonoBehaviour
{
    [SerializeField] private Image customerIcon;
    [SerializeField] private Image patienceBar;
    [SerializeField] private Transform orderContainer;

    [SerializeField] private GameObject itemIconPrefab;

    private Customer_New targetCustomer;
    private List<GameObject> itemIcons = new();

    public void ConnectUI(Customer_New customer)
    {
        targetCustomer = customer;
        customerIcon.sprite = customer.data.icon;

        customer.patience.OnPatienceChanged += UpdatePatience;
        customer.orderSystem.OnOrderGenerated += UpdateOrderIcons;
        customer.orderSystem.OnItemServed += HandleItemServed;
    }

    void OnDestroy()
    {
        if (targetCustomer != null)
        {
            targetCustomer.patience.OnPatienceChanged -= UpdatePatience;
            targetCustomer.orderSystem.OnOrderGenerated -= UpdateOrderIcons;
            targetCustomer.orderSystem.OnItemServed -= HandleItemServed;
        }
    }

    private void UpdatePatience(float normalizedPatience)
    {
        patienceBar.fillAmount = normalizedPatience;
    }

    private void UpdateOrderIcons(List<ItemData> items)
    {
        foreach (var icon in itemIcons) Destroy(icon);
        itemIcons.Clear();

        foreach (var item in items)
        {
            GameObject iconObj = Instantiate(itemIconPrefab, orderContainer);
            iconObj.GetComponent<Image>().sprite = item.icon;
            itemIcons.Add(iconObj);
        }
    }
    
    private void HandleItemServed(ItemData servedItem)
    {
        for(int i = 0; i < itemIcons.Count; i++)
        {
            if(itemIcons[i].GetComponent<Image>().sprite == servedItem.icon)
            {
                Destroy(itemIcons[i]);
                itemIcons.RemoveAt(i);
                break;
            }
        }
    }
}
