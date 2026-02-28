using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private TMP_Text moneyUI;
    [SerializeField] private TMP_Text moneyChangedText;
    [SerializeField] private TMP_Text eventText;
    [SerializeField] private GameObject pauseUI;

    [Header("Player")]
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private GameObject healthUIPanel;
    [SerializeField] private List<Image> healthUI = new();

    [SerializeField] private Image interactHoldProgress;

    private void Awake()
    {
        GameManager.instance.RegisterUI(this);

        playerUI = FindAnyObjectByType<PlayerUI>();
        playerUI.RegisterUI(this);
    }

    public TMP_Text MoneyUI => moneyUI;
    public TMP_Text MoneyChangedText => moneyChangedText;
    public TMP_Text EventText => eventText;
    public GameObject PauseUI => pauseUI;

    public GameObject HealthUIPanel => healthUIPanel;
    public List<Image> HealthUI => healthUI;

    public Image InteractHoldProgress => interactHoldProgress;
}
