using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DayManager : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text quotaText;

    [SerializeField] private GameObject dayEndUI;
    [SerializeField] private Button nextDayButton;
    [SerializeField] private Button mainMenuButton;

    private int _currentDay = 1;
    public int CurrentDay
    {
        get => _currentDay;
        set => _currentDay = Mathf.Clamp(value, 1, maxDays);
    }

    private int maxDays = 7;
    private int currentQuota;

    [SerializeField] private Timer timer;

    private GameObject player;
    private PlayerMoney playerMoney;
    private PlayerInput Input;

    void OnEnable()
    {
        timer.OnTimerRunOut += DayEnded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        timer.OnTimerRunOut -= DayEnded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        if (scene.name == "MainGame")
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        timer = FindAnyObjectByType<Timer>();

        if (nextDayButton != null)
            nextDayButton.onClick.AddListener(NextDay);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMenu);

        player = GameObject.FindWithTag(PlayerConstants.PLAYER_TAG);
        if (player == null)
        {
            Debug.Log("No player found in scene");
            return;
        }

        playerMoney = player.GetComponent<PlayerMoney>();
        Input = player.GetComponent<PlayerInput>();
    }

    void Start()
    {
        dayEndUI.SetActive(false);
        dayText.text = $"Day: {CurrentDay}";

        Debug.Log("Start called");
    }

    public void SetUI(TMP_Text dayText,
    TMP_Text quotaText,
    GameObject dayEndUI,
    Button nextDayButton,
    Button mainMenuButton)
    {
        this.dayText = dayText;
        this.quotaText = quotaText;
        this.dayEndUI = dayEndUI;
        this.nextDayButton = nextDayButton;
        this.mainMenuButton = mainMenuButton;
    }

    private void DayEnded()
    {
        Time.timeScale = 0f;
        dayEndUI.SetActive(true);

        Input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_UI);
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
        dayEndUI.SetActive(false);
        Time.timeScale = 1f;
        Input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_PLAYER);

        SceneLoader.RestartScene();
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.ChangeScene("StartScene");
    }
}