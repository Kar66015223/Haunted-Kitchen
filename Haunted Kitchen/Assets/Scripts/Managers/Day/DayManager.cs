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
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMoney playerMoney;

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
        
        if(timer != null)
            timer.OnTimerRunOut += EndDay;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (timer != null)
            timer.OnTimerRunOut -= EndDay;
    }

    void Start()
    {
        if(playerMoney != null)
        {
            AddMoney(firstDayMoney);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainGame")
        {
            Destroy(gameObject);
            return;
        }

        timer = FindAnyObjectByType<Timer>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        playerMoney = FindAnyObjectByType<PlayerMoney>();

        if (timer != null)
        {
            timer.OnTimerRunOut -= EndDay;
            timer.OnTimerRunOut += EndDay;
        }
        
        OnDayStarted?.Invoke(CurrentDay);
    }

    private void EndDay()
    {
        Time.timeScale = 0f;
        timer.ResetTime();

        OnDayEnded?.Invoke(CurrentDay);

        if (playerInput != null)
            playerInput.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_UI);
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
    
    private void AddMoney(int amount)
    {
        playerMoney.ChangeMoneyAmount(amount);
    }
}