using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class QuotaManager : MonoBehaviour
{
    public static QuotaManager Instance { get; private set; }
    [SerializeField] private DayManager dayManager;

    [SerializeField] private int currentQuota = 5000;
    public int CurrentQuota => currentQuota;

    private int quotaIncrease = 3000;
    private int quotaIncreaseLastDay = 5000;

    public PlayerMoney playerMoney { get; private set; }

    public event Action<int> OnQuotaChanged;

    void Awake()
    {
        dayManager = DayManager.Instance;
        
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
        
        if (dayManager == null)
            dayManager = DayManager.Instance;

        Invoke(nameof(OnDayStartedSub), 0.1f);
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        dayManager.OnDayStarted -= IncreaseQuota;
    }

    void Start()
    {
        if (dayManager == null)
            dayManager = DayManager.Instance;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainGame")
        {
            Destroy(gameObject);
            return;
        }

        playerMoney = FindAnyObjectByType<PlayerMoney>();
    }

    public void ChangeQuotaAmount(int amount)
    {
        currentQuota += amount;
        OnQuotaChanged?.Invoke(currentQuota);
    }

    private void IncreaseQuota(int amount)
    {
        if (amount == dayManager.MaxDays)
        {
            ChangeQuotaAmount(quotaIncreaseLastDay);
            return;
        }

        ChangeQuotaAmount(quotaIncrease);
    }

    void OnDayStartedSub()
    {
        dayManager.OnDayStarted += IncreaseQuota;
    }
}
