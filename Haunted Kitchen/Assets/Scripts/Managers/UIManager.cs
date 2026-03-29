using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    [Header("Player")]
    [SerializeField] private PlayerUI playerUI; 
    [SerializeField] private GameObject healthUIPanel;
    [SerializeField] private List<Image> healthUI = new();

    private void Awake()
    {
        if (moneyUIManager == null) moneyUIManager = FindAnyObjectByType<MoneyUIManager>();
        if (eventTextUI == null) eventTextUI = FindAnyObjectByType<EventTextUI>();

        if (playerUI == null) playerUI = FindAnyObjectByType<PlayerUI>();
        
        eventTextUI?.SetUI(eventText);

        playerUI?.SetUI(healthUIPanel);
    }
}
