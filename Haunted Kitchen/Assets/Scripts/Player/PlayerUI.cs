using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private List<Image> healthUI = new();

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthUI;
    }
    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthUI;
    }

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void RegisterUI(UIManager ui)
    {
        uiPanel = ui.HealthUIPanel;
        healthUI = ui.HealthUI;
    }

    private void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < healthUI.Count; i++)
        {
            healthUI[i].enabled = i < currentHealth;
        }
    }
}
