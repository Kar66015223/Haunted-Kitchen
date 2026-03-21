using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("Game")]

    //Money
    [SerializeField] private TMP_Text moneyUI;
    [SerializeField] private TMP_Text moneyChangedText;
    [SerializeField] private MoneyUIManager moneyUIManager;

    //Event
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private EventTextUI eventTextUI;

    //Pause
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private PauseManager pauseManager;

    // Day
    [SerializeField] private DayManager dayManager;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text quotaText;

    [SerializeField] private GameObject dayEndUI;
    [SerializeField] private Button nextDayButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Player")]
    [SerializeField] private PlayerUI playerUI; 
    [SerializeField] private GameObject healthUIPanel;
    [SerializeField] private List<Image> healthUI = new();

    [SerializeField] private Image interactHoldProgress;

    private void Awake()
    {
        if (moneyUIManager == null) moneyUIManager = FindAnyObjectByType<MoneyUIManager>();
        if (eventTextUI == null) eventTextUI = FindAnyObjectByType<EventTextUI>();
        if (pauseManager == null) pauseManager = FindAnyObjectByType<PauseManager>();
        if (dayManager == null) dayManager = FindAnyObjectByType<DayManager>();

        if (playerUI == null) playerUI = FindAnyObjectByType<PlayerUI>();

        moneyUIManager?.SetUI(moneyUI, moneyChangedText);
        eventTextUI?.SetUI(eventText);
        pauseManager?.SetUI(pauseUI);
        dayManager?.SetUI(dayText, quotaText, dayEndUI, nextDayButton, mainMenuButton);

        playerUI?.SetUI(healthUIPanel, interactHoldProgress);
    }
}
