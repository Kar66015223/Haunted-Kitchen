using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoneyUIManager : MonoBehaviour
{
    public static MoneyUIManager Instance;

    [SerializeField] private TMP_Text moneyUI;
    public TMP_Text MoneyUI => moneyUI;

    [SerializeField] private TMP_Text moneyChangedText;

    [SerializeField] private FadeOutText moneyChangedFadeOut;
    private Coroutine fadeCo;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        MoneyManager.OnMoneyChanged += HandleMoneyChanged;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        MoneyManager.OnMoneyChanged -= HandleMoneyChanged;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        moneyChangedFadeOut = moneyChangedText.GetComponent<FadeOutText>();

        if (moneyChangedFadeOut == null)
            Debug.LogError($"FadeOutText not found in {moneyChangedText.gameObject.name}");
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "MainGame")
        {
            Destroy(gameObject);
        }
    }

    private void HandleMoneyChanged(int newMoney, int amountChanged)
    {
        if (moneyUI == null) return;

        UpdateMoneyDisplay(newMoney);
        ShowMoneyChange(amountChanged);
    }

    private void UpdateMoneyDisplay(int money)
    {
        moneyUI.text = $"Money: {money} $";
    }

    private void ShowMoneyChange(int amountChanged)
    {
        if (amountChanged > 0)
        {
            moneyChangedText.text = $"{amountChanged}$";
            moneyChangedText.color = Color.green;
        }
        else
        {
            moneyChangedText.text = $"{amountChanged}$";
            moneyChangedText.color = Color.red;
        }

        if (fadeCo != null)
            StopCoroutine(fadeCo);

        if (moneyChangedFadeOut != null)
        {
            moneyChangedFadeOut.SetAlphaToFull();
            fadeCo = StartCoroutine(moneyChangedFadeOut.FadeOut());
        }
    }
}
