using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

    private int _currentDay = 1;
    public int CurrentDay
    {
        get => _currentDay;
        set => _currentDay = Mathf.Clamp(value, 1, maxDays);
    }
    private int maxDays = 7;
    public int MaxDays => maxDays;

    [SerializeField] private int firstDayMoney = 5000;

    [SerializeField] private Timer timer;
    public Timer Timer => timer;

    [SerializeField] private CustomerSpawner spawner; 
    private bool isWaitingForCustomers = false;

    [SerializeField] private PlayerInput playerInput;

    public event Action<int> OnDayEnded;
    public event Action<int> OnDayStarted;

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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameEvents.OnAddMoneyButtonClicked += OnAddMoneyClicked;
        
        if(timer != null)
            timer.OnTimerRunOut += StartClosingPeriod;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameEvents.OnAddMoneyButtonClicked -= OnAddMoneyClicked;

        if (timer != null)
            timer.OnTimerRunOut -= StartClosingPeriod;
    }

    void Start()
    {
        if (MoneyManager.Instance != null)
        {
            AddMoney(firstDayMoney);
        }
    }

    void Update()
    {
        if (isWaitingForCustomers)
        {
            if(spawner != null && spawner.ActiveCustomerCount <= 0)
            {
                isWaitingForCustomers = false;
                EndDay();
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainGame")
        {
            Destroy(gameObject);
            return;
        }

        timer = FindAnyObjectByType<Timer>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        spawner = FindAnyObjectByType<CustomerSpawner>();

        if (timer != null)
        {
            timer.OnTimerRunOut -= StartClosingPeriod;
            timer.OnTimerRunOut += StartClosingPeriod;
        }

        isWaitingForCustomers = false;
        OnDayStarted?.Invoke(CurrentDay);
    }

    private void StartClosingPeriod()
    {
        isWaitingForCustomers = true;

        if (spawner != null && spawner.ActiveCustomerCount <= 0)
        {
            isWaitingForCustomers = false;
            EndDay();
        }

        GameEvents.OnShowEventText("The day will end when all customers has left", Color.red);
    }

    private void EndDay()
    {
        if (CurrentDay == maxDays)
        {
            int currentQuota = QuotaManager.Instance.CurrentQuota;
            bool gotGoodEnd = MoneyManager.Instance.CurrentMoney >= currentQuota
                ? true : false;

            EnterEndingScene(gotGoodEnd ?
                SceneConstants.ENDING_GOODEND_NAME : SceneConstants.ENDING_BADEND_NAME);
        }

        Worker worker = FindAnyObjectByType<Worker>();
        if(worker != null)
            Destroy(worker);

        Time.timeScale = 0f;
        timer.ResetTime();

        OnDayEnded?.Invoke(CurrentDay);

        GameEvents.OnUIShow?.Invoke(true);
    }

    public void NextDay()
    {
        ChangeDay(CurrentDay + 1);
    }

    public void ResetDay()
    {
        ChangeDay(1);
    }

    public void ChangeDay(int targetDay)
    {
        CurrentDay = targetDay;
        Time.timeScale = 1f;

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_PLAYER);

        SceneLoader.RestartScene();
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.ChangeScene("StartScene");
    }

    public void EnterEndingScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneLoader.ChangeScene(sceneName);
    }

    public void OnAddMoneyClicked()
    {
        AddMoney(firstDayMoney);
    }
    
    private void AddMoney(int amount)
    {
        MoneyManager.Instance.ChangeMoneyAmount(amount);
    }
}