using System.Collections;
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
    private int money;

    [Header("Money UI")]
    public TMP_Text moneyUI;
    public TMP_Text moneyChangedText;

    [Header("Event Text")]
    public TMP_Text eventText;

    [Header("Text Fading")]
    [SerializeField] private float fadeDuration = 2f;
    private Coroutine moneyChangedFadeCo;
    private Coroutine eventTextFadeCo;
    
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

        UIManager ui = FindAnyObjectByType<UIManager>();
        if (ui != null)
        {
            RegisterUI(ui);
        }

        playerMoney = FindAnyObjectByType<PlayerMoney>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        money = 0;

        isPaused = false;
        Time.timeScale = 1f;
        pauseUI?.SetActive(false);
    }

    public void RegisterUI(UIManager ui)
    {
        moneyUI = ui.MoneyUI;
        moneyChangedText = ui.MoneyChangedText;
        eventText = ui.EventText;
        pauseUI = ui.PauseUI;
    }

    void HandleMoneyChanged(int newMoney, int amountChanged)
    {
        money = newMoney;
        UpdateMoneyUI();

        ShowMoneyChangedText(amountChanged);

        if (moneyChangedFadeCo != null)
        {
            StopCoroutine(moneyChangedFadeCo);
        }
        SetAlphaToFull(moneyChangedText);
        moneyChangedFadeCo = StartCoroutine(FadeOutText(moneyChangedText));
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

    public void ShowMoneyChangedText(int amountChanged)
    {
        if (amountChanged > 0)
        {
            moneyChangedText.text = $"+{amountChanged}$";
            moneyChangedText.color = Color.green;
        }
        else
        {
            moneyChangedText.text = $"{amountChanged}$";
            moneyChangedText.color = Color.red;
        }
    }

    public void ShowEventText()
    {
        if (eventTextFadeCo != null)
        {
            StopCoroutine(eventTextFadeCo);
        }
        SetAlphaToFull(eventText);
        eventTextFadeCo = StartCoroutine(FadeOutText(eventText));
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

    IEnumerator FadeOutText(TMP_Text text)
    {
        float fadeElapsed = 0f;

        Color c = text.color;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);
            text.color = c;
            yield return null;
        }

        c.a = 0f;
        text.color = c;
    }

    public void SetAlphaToFull(TMP_Text text)
    {
        Color c = text.color;
        c.a = 1f;
        text.color = c;
    }
}
