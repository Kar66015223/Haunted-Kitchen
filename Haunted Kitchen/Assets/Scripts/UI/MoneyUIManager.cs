using TMPro;
using UnityEngine;

public class MoneyUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyUI;
    [SerializeField] private TMP_Text moneyChangedText;

    [SerializeField] private FadeOutText moneyChangedFadeOut;
    private Coroutine fadeCo;

    void OnEnable()
    {
        PlayerMoney.OnMoneyChanged += HandleMoneyChanged;
    }
    void OnDisable()
    {
        PlayerMoney.OnMoneyChanged -= HandleMoneyChanged;
    }

    void Awake()
    {
        moneyChangedFadeOut = moneyChangedText.GetComponent<FadeOutText>();

        if (moneyChangedFadeOut == null)
            Debug.LogError($"FadeOutText not found in {moneyChangedText.gameObject.name}");
    }

    public void SetUI(TMP_Text money, TMP_Text changed)
    {
        moneyUI = money;
        moneyChangedText = changed;
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
