using UnityEngine;
using TMPro;

public class PlayerInteractionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text interactPrompt;
    [SerializeField] private PlayerInteractableDetector detector;

    [SerializeField] private PlayerInteractionHandler handler;

    private Outline currentOutline;

    void Awake()
    {
        detector = GetComponent<PlayerInteractableDetector>();
        handler = GetComponent<PlayerInteractionHandler>();

        if (detector == null)
            Debug.Log("PlayerInteractableDetector not found!");
        if (handler == null)
            Debug.Log("InteractionHandler not found!");
    }

    void OnEnable()
    {
        detector.OnInteractableDetected += OnInteractableDetected;
        detector.OnInteractableLost += OnInteractablelost;
    }

    void OnDisable()
    {
        detector.OnInteractableDetected -= OnInteractableDetected;
        detector.OnInteractableLost -= OnInteractablelost;
    }

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.enabled = false;
        }
    }

    void Update()
    {
        if (detector.GetCurrentInteractable() != null)
        {
            bool canInteract = handler.CanInteractWithCurrent();

            if (canInteract && !interactPrompt.enabled)
            {
                ShowPrompt();
            }

            else if (!canInteract && interactPrompt.enabled)
            {
                HidePrompt();
            }
        }
    }

    private void OnInteractableDetected(Iinteractable interactable)
    {
        if (handler.CanInteractWithCurrent())
        {
            ShowPrompt();
        }
    }
    
    private void OnInteractablelost()
    {
        HidePrompt();
    }

    private void ShowPrompt()
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
