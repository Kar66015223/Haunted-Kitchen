using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerInputHandler inputHandler;

    [Header("Health")]
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private List<Image> healthUI = new();

    [Header("Hold Interact")]
    [SerializeField] private Image interactHoldProgress;
    private bool canInteractNow = false;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthUI;
        inputHandler.OnHoldProgressChanged += UpdateHoldProgress;
        PlayerInteractionHandler.OnHoldInteractValidityCheck += SetCanInteract;
    }
    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthUI;
        inputHandler.OnHoldProgressChanged -= UpdateHoldProgress;
        PlayerInteractionHandler.OnHoldInteractValidityCheck -= SetCanInteract;
    }

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void Start()
    {
        healthUI = uiPanel.GetComponentsInChildren<Image>().ToList();
    }

    public void SetUI(GameObject panel, Image holdProgress)
    {
        uiPanel = panel;
        interactHoldProgress = holdProgress;
    }

    private void UpdateHealthUI(int currentHealth)
    {
        for (int i = 0; i < healthUI.Count; i++)
        {
            healthUI[i].enabled = i < currentHealth;
        }
    }

    private void SetCanInteract(bool canInteract)
    {
        canInteractNow = canInteract;

        if(!canInteract)
        {
            interactHoldProgress.fillAmount = 0f;
        }
    }

    private void UpdateHoldProgress(float progress)
    {
        if (interactHoldProgress != null && canInteractNow)
            interactHoldProgress.fillAmount = progress;
    }
}
