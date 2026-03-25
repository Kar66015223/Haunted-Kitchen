using System;
using UnityEngine;

public class RepairKit : MonoBehaviour
{
    [SerializeField] private int amount;
    [SerializeField] private int maxAmount = 3;

    private ItemAmountText ui;

    void Awake()
    {
        ui = GetComponentInChildren<ItemAmountText>();
    }

    void Start()
    {
        amount = maxAmount;
        ui.UpdateAmountText(amount);
    }

    void Update()
    {
        Debug.Log(amount);
        if (amount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Repair(IDestroyable destroyable)
    {
        Debug.Log("repaired");
        destroyable.SetStationStatus(StationStatus.Usable);
        amount--;

        ui.UpdateAmountText(amount);
    }
}
