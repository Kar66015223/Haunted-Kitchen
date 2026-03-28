using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayUI : MonoBehaviour
{
    private DayManager dayManager;

    [SerializeField] private TMP_Text dayText;
    [SerializeField] private GameObject dayEndedUI;

    [SerializeField] private Button nextDayButton;
    [SerializeField] private Button mainMenubutton;

    void Awake()
    {
        dayManager = DayManager.Instance;
    }

    void OnEnable()
    {
        // Give time to find DayManager first
        Invoke(nameof(OnDayEndedSub), 0.1f);
        Invoke(nameof(OnDayStartedSub), 0.1f);
    }

    void OnDisable()
    {
        if (dayManager != null)
        {
            dayManager.OnDayEnded -= OnDayEnded;
            dayManager.OnDayStarted -= OnDayStarted;
        }

        if (nextDayButton != null)
            nextDayButton.onClick.RemoveAllListeners();

        if (mainMenubutton != null)
            mainMenubutton.onClick.RemoveAllListeners();
    }

    void Start()
    {
        if (dayManager == null)
            dayManager = DayManager.Instance;

        if (dayManager == null) return;

        nextDayButton?.onClick.AddListener(dayManager.NextDay);
        mainMenubutton?.onClick.AddListener(dayManager.ReturnToMenu);

        OnDayStarted(dayManager.CurrentDay);
    }

    private void OnDayStarted(int day)
    {
        dayEndedUI.SetActive(false);
        dayText.text = $"Day: {day}";
    }

    private void OnDayEnded(int day)
    {
        dayEndedUI.SetActive(true);
        dayText.text = $"Day: {day}";
    }

    void OnDayEndedSub()
    {
        if(dayManager != null)
            dayManager.OnDayEnded += OnDayEnded;
    }
    void OnDayStartedSub()
    {
        if(dayManager != null)
            dayManager.OnDayStarted += OnDayStarted;
    }
}
