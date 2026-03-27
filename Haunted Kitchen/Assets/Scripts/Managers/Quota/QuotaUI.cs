using TMPro;
using UnityEngine;

public class QuotaUI : MonoBehaviour
{
    [SerializeField] private QuotaManager quotaManager;
    [SerializeField] private TMP_Text quotaText;

    void Awake()
    {
        quotaManager = QuotaManager.Instance;
    }

    void OnEnable()
    {
        if (quotaManager == null)
        quotaManager = QuotaManager.Instance;

        if (quotaManager != null)
            quotaManager.OnQuotaChanged += UpdateQuotaText;
    }

    void OnDisable()
    {
        quotaManager.OnQuotaChanged -= UpdateQuotaText;
    }

    void Start()
    {
        if (quotaManager != null)
            UpdateQuotaText(quotaManager.CurrentQuota);
    }

    void UpdateQuotaText(int currentQuota)
    {
        quotaText.text = $"Quota:\n{quotaManager.playerMoney.currentMoney}/{currentQuota}";
    }
}
