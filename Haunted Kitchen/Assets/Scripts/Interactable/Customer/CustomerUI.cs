using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CustomerUI : MonoBehaviour
{
    [Header("Order")]
    [SerializeField] private Image idleUI;
    [SerializeField] private GameObject orderIconPrefab;
    [SerializeField] private Transform orderContainer;

    private List<GameObject> spawnedIcons = new();

    [Header("Patience")]
    [SerializeField] private Image patienceImg;

    private CustomerOrder order;
    private CustomerPatience patience;
    private CustomerMovement movement;
    private Customer_New customer;

    [SerializeField] private CustomerState currentState;

    private void Awake()
    {
        order = GetComponent<CustomerOrder>();
        patience = GetComponent<CustomerPatience>();
        customer = GetComponent<Customer_New>();
        movement = GetComponent<CustomerMovement>();
    }

    private void OnEnable()
    {
        order.OnOrderGenerated += DisplayOrder;
        patience.OnPatienceChanged += UpdatePatience;
        customer.OnStateChanged += HandleStateChanged;
        movement.OnArrived += UpdateUI;
        order.OnItemServed += RemoveServedIcon;
    }

    private void OnDisable()
    {
        order.OnOrderGenerated -= DisplayOrder;
        patience.OnPatienceChanged -= UpdatePatience;
        customer.OnStateChanged -= HandleStateChanged;
        movement.OnArrived -= UpdateUI;
        order.OnItemServed -= RemoveServedIcon;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void HandleStateChanged(CustomerState state)
    {
        currentState = state;
        UpdateUI();
    }

    private void UpdateUI()
    {
        bool arrived = movement.IsArrived;

        idleUI.gameObject.SetActive(currentState == CustomerState.Idle && arrived);

        patienceImg.gameObject.SetActive(currentState != CustomerState.Leaving && arrived);
    }

    private void UpdatePatience(float normalized)
    {
        patienceImg.fillAmount = normalized;
    }

    private void DisplayOrder(List<ItemData> items)
    {
        ClearOrderUI();

        foreach (ItemData item in items)
        {
            GameObject iconObj = Instantiate(orderIconPrefab, orderContainer);

            Image img = iconObj.GetComponent<Image>();
            img.sprite = item.icon;

            spawnedIcons.Add(iconObj);
        }
    }

    private void RemoveServedIcon(ItemData servedItem)
    {
        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            Image img = spawnedIcons[i].GetComponent<Image>();

            if (img.sprite == servedItem.icon)
            {
                Destroy(spawnedIcons[i]);
                spawnedIcons.RemoveAt(i);
                break;
            }
        }
    }

    private void ClearOrderUI()
    {
        foreach (GameObject obj in spawnedIcons)
        {
            Destroy(obj);
        }

        spawnedIcons.Clear();
    }
}
