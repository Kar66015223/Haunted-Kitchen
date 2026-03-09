using UnityEngine;
using TMPro;
using System;
using UnityEngine.Analytics;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text interactPrompt;
    [SerializeField] private PlayerInteractableDetector detector;

    private Outline currentOutline;

    void Awake()
    {
        detector = GetComponent<PlayerInteractableDetector>();

        if (detector == null)
            Debug.Log("PlayerInteractableDetector not found!");
    }

    void OnEnable()
    {
        detector.OnInteractableDetected += ShowPrompt;
        detector.OnInteractableLost += HidePrompt;
    }

    void OnDisable()
    {
        detector.OnInteractableDetected -= ShowPrompt;
        detector.OnInteractableLost -= HidePrompt;
    }

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.enabled = false;
        }
    }

    private void ShowPrompt(Iinteractable interactable)
    {
        if (interactPrompt != null)
        {
            interactPrompt.enabled = true;
        }

        var mb = detector.GetCurrentInteractableMB();
        if (mb != null)
        {
            SetOutline(mb);
        }
    }

    private void HidePrompt()
    {
        if (interactPrompt != null)
        {
            interactPrompt.enabled = false;
        }
        ClearOutline();
    }

    private void SetOutline(MonoBehaviour interactableMB)
    {
        if (interactableMB == null) return;

        var outline = interactableMB.GetComponentInChildren<Outline>();
        if (outline == null)
        {
            Debug.LogWarning($"No outline found on {interactableMB.gameObject.name}");
            return;
        }

        currentOutline = outline;
        currentOutline.enabled = true;
    }
    
    private void ClearOutline()
    {
        if(currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
}
