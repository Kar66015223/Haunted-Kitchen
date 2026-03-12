using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private TMP_Text moneyUI;
    [SerializeField] private TMP_Text moneyChangedText;
    [SerializeField] private MoneyUIManager moneyUIManager;

    [SerializeField] private TMP_Text eventText;
    [SerializeField] private EventTextUI eventTextUI;

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private PauseManager pauseManager;

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

        if (playerUI == null) playerUI = FindAnyObjectByType<PlayerUI>();

        moneyUIManager?.SetUI(moneyUI, moneyChangedText);
        eventTextUI?.SetUI(eventText);
        pauseManager?.SetUI(pauseUI);

        playerUI?.SetUI(healthUIPanel, interactHoldProgress);
    }

    public TMP_Text MoneyUI => moneyUI;
    public TMP_Text MoneyChangedText => moneyChangedText;
    public TMP_Text EventText => eventText;
    public GameObject PauseUI => pauseUI;

    // public GameObject HealthUIPanel => healthUIPanel;
    // public List<Image> HealthUI => healthUI;

    // public Image InteractHoldProgress => interactHoldProgress;
}
