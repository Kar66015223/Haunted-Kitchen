using UnityEngine;
using System;

public class PlayerMoney : MonoBehaviour
{   
    public int currentMoney { get; private set; }
    public static event Action<int, int> OnMoneyChanged; // int newMoney, int amountChanged

    public void ChangeMoneyAmount(int amount)
    {
        currentMoney = Mathf.Max(0, currentMoney + amount);
        OnMoneyChanged?.Invoke(currentMoney, amount);
    }
}
