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
        if (quotaManager != null)
            quotaManager.OnQuotaChanged -= UpdateQuotaText;
    }

    void Start()
    {
        if (quotaManager == null)
        {
            quotaManager = QuotaManager.Instance;
            UpdateQuotaText(quotaManager.CurrentQuota);
        }
    }

    void Update()
    {
        if (MoneyManager.Instance.CurrentMoney >= quotaManager.CurrentQuota)
        {
            quotaText.color = Color.green;
        }
        else
        {
            quotaText.color = Color.red;
        }
    }

    void UpdateQuotaText(int currentQuota)
    {
        quotaText.text = $"Quota:\n{currentQuota}";
    }
}
