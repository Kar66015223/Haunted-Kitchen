using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerMoney playerMoney;
    public PlayerInput playerInput;

    [Header("Money")]
    public int money;

    [Header("Money UI")]
    public TMP_Text moneyUI;

    [Header("Pause UI")]
    public GameObject pauseUI;
    public Button resumeButton;
    public Button MainMenuButton;

    public bool isPaused = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        PlayerMoney.OnMoneyChanged += HandleMoneyChanged;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        PlayerMoney.OnMoneyChanged -= HandleMoneyChanged;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainGame") 
            return;

        moneyUI = GameObject.FindWithTag("MoneyUI")?.GetComponent<TMP_Text>();
        pauseUI = GameObject.FindWithTag("PauseUI");

        playerMoney = FindAnyObjectByType<PlayerMoney>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        money = 0;

        isPaused = false;
        Time.timeScale = 1f;
        pauseUI?.SetActive(false);
    }

    void HandleMoneyChanged(int newMoney)
    {
        money = newMoney;
        UpdateMoneyUI();
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerMoney = FindAnyObjectByType<PlayerMoney>();
        pauseUI?.SetActive(false);
    }

    public void UpdateMoneyUI()
    {
        moneyUI.text = $"Money: {money} $";
    }

    public void Pause()
    {
        Debug.Log("PAUSE CALLED");
        isPaused = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0f;

        playerInput.SwitchCurrentActionMap("UI");
    }

    public void UnPause()
    {
        Debug.Log("Resume pressed: " + Time.frameCount);
        isPaused = false;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void ChangeSceneToStartScene()
    {
        SceneLoader.ChangeScene("StartScene");
    }
}
