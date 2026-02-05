using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerMoney playerMoney;

    [Header("Money")]
    public int money;

    [Header("Money UI")]
    public TMP_Text moneyUI;

    [Header("Pause UI")]
    public GameObject pauseUI;
    public bool isPaused = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainGame") 
            return;

        moneyUI = GameObject.FindWithTag("MoneyUI")?.GetComponent<TMP_Text>();
        pauseUI = GameObject.FindWithTag("PauseUI");
        playerMoney = FindAnyObjectByType<PlayerMoney>();
        money = 0;

        isPaused = false;
        Time.timeScale = 1f;
        pauseUI?.SetActive(false);
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

    private void Update()
    {
        if (playerMoney != null && money != playerMoney.currentMoney)
        {
            money = playerMoney.currentMoney;
            UpdateMoneyUI();
        }
    }

    public void UpdateMoneyUI()
    {
        moneyUI.text = $"Money: {money} $";
    }

    public void Pause()
    {
        isPaused = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UnPause()
    {
        isPaused = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ChangeSceneToStartScene()
    {
        SceneLoader.ChangeScene("StartScene");
    }
}
