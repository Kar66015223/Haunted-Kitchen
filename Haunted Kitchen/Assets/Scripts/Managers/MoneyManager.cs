using System;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private int _currentMoney;
    public int CurrentMoney
    {
        get => _currentMoney;
        private set => _currentMoney = Mathf.Max(0, value);
    }

    public event Action<int, int> OnMoneyChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeMoneyAmount(int value)
    {
        CurrentMoney += value;
        OnMoneyChanged?.Invoke(CurrentMoney, value);
    }
}
