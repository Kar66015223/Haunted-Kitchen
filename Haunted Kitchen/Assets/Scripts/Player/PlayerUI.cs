using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerController controller;

    [Header("Health")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private List<Image> healthUI = new();

    [Header("Hold Interact")]
    [SerializeField] private Image interactHoldProgress;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthUI;
        controller.OnHoldProgressChanged += UpdateHoldProgress;
    }
    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthUI;
        controller.OnHoldProgressChanged -= UpdateHoldProgress;
    }

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        controller = GetComponent<PlayerController>();
    }

    public void RegisterUI(UIManager ui)
    {
        uiPanel = ui.HealthUIPanel;
        healthUI = ui.HealthUI;
        interactHoldProgress = ui.InteractHoldProgress;
    }

    private void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < healthUI.Count; i++)
        {
            healthUI[i].enabled = i < currentHealth;
        }
    }

    private void UpdateHoldProgress(float progress)
    {
        if (interactHoldProgress != null)
            interactHoldProgress.fillAmount = progress;
    }
}
