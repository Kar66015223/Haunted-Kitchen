using UnityEngine;
using System;

public class PlayerMoney : MonoBehaviour
{
    public int currentMoney;

    public static event Action<int, int> OnMoneyChanged;

    private void Start()
    {
        ChangeMoneyAmount(5000);
    }

    public void ChangeMoneyAmount(int amount)
    {
        currentMoney = Mathf.Max(0, currentMoney + amount);
        OnMoneyChanged?.Invoke(currentMoney, amount);
    }
}
