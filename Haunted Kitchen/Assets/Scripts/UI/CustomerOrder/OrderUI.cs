using System.Collections.Generic;
using UnityEngine;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private Dictionary<Customer_New, GameObject> activePanels = new();

    void OnEnable()
    {
        GameEvents.OnCustomerArrived += AddCustomerPanel;
    }

    void OnDestroy()
    {
        GameEvents.OnCustomerArrived -= AddCustomerPanel;
    }

    private void AddCustomerPanel(Customer_New customer)
    {
        if (activePanels.ContainsKey(customer))
            return;

        GameObject panel = Instantiate(panelPrefab, container);
        activePanels.Add(customer, panel);

        panel.GetComponent<OrderPanelUI>().ConnectUI(customer);

        customer.OnFinished += c => RemoveCustomerPanel(customer);
    }

    private void RemoveCustomerPanel(Customer_New customer)
    {
        if(activePanels.ContainsKey(customer))
        {
            Destroy(activePanels[customer]);
            activePanels.Remove(customer);
        }
    }
}
