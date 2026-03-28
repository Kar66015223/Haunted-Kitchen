using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [SerializeField] private int _currentMoney;
    public int CurrentMoney
    {
        get => _currentMoney;
        private set => _currentMoney = Mathf.Max(0, value);
    }

    public static event Action<int, int> OnMoneyChanged;

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainGame")
        {
            ChangeMoneyAmount(-CurrentMoney);
            Destroy(gameObject);
        }
    }

    public void ChangeMoneyAmount(int value)
    {
        CurrentMoney += value;
        OnMoneyChanged?.Invoke(CurrentMoney, value);
    }
}
