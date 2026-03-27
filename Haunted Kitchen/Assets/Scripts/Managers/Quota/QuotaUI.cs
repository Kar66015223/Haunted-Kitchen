using TMPro;
using UnityEngine;

public class QuotaUI : MonoBehaviour
{
    private QuotaManager quotaManager;

    [SerializeField] private TMP_Text quotaText;

    void Awake()
    {
        quotaManager = QuotaManager.Instance;
    }

    void OnEnable()
    {
        Invoke(nameof(OnQuotaChangedSub), 0.1f);
    }

    void OnDisable()
    {
        quotaManager.OnQuotaChanged -= UpdateQuotaText;
    }

    void OnQuotaChangedSub()
    {
        quotaManager.OnQuotaChanged += UpdateQuotaText;
    }

    void UpdateQuotaText(int currentQuota)
    {
        quotaText.text = $"Quota:\nCurrentMoney/Quota";
    }
}
