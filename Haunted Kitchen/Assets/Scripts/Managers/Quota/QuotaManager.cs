using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuotaManager : MonoBehaviour
{
    public static QuotaManager Instance { get; private set; }

    private int currentQuota = 5000;
    private PlayerMoney playerMoney;

    public event Action<int> OnQuotaChanged;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainGame")
        {
            Destroy(gameObject);
            return;
        }

        playerMoney = FindAnyObjectByType<PlayerMoney>();
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void ChangeQuotaAmount(int amount)
    {
        currentQuota += amount;
        OnQuotaChanged?.Invoke(currentQuota);
    }
}
