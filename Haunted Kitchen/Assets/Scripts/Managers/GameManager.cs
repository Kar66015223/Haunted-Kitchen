using System.Collections;
using System.Net.NetworkInformation;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
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
    public TMP_Text moneyChangedText;

    [SerializeField] private float fadeDuration = 2f;
    private Coroutine fadeCo;

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
        moneyChangedText = GameObject.FindWithTag("MoneyChangedText")?.GetComponent<TMP_Text>();

        pauseUI = GameObject.FindWithTag("PauseUI");

        playerMoney = FindAnyObjectByType<PlayerMoney>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        money = 0;

        isPaused = false;
        Time.timeScale = 1f;
        pauseUI?.SetActive(false);
    }

    void HandleMoneyChanged(int newMoney, int amountChanged)
    {
        money = newMoney;
        UpdateMoneyUI();

        ShowMoneyChangedText(amountChanged);

        if (fadeCo != null)
        {
            StopCoroutine(fadeCo);
        }
        SetAlphaToFull();
        fadeCo = StartCoroutine(FadeOutText());
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

    IEnumerator FadeOutText()
    {
        float fadeElapsed = 0f;

        Color c = moneyChangedText.color;

        while (fadeElapsed < fadeDuration)
        {
            fadeElapsed += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, fadeElapsed / fadeDuration);
            moneyChangedText.color = c;
            yield return null;
        }

        c.a = 0f;
        moneyChangedText.color = c;
    }

    public void SetAlphaToFull()
    {
        Color c = moneyChangedText.color;
        c.a = 1f;
        moneyChangedText.color = c;
    }
}
