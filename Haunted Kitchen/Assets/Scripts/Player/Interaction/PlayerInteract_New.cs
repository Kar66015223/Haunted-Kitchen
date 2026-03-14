using UnityEngine;

public class PlayerInteract_New : MonoBehaviour
{
    [SerializeField] private PlayerInteractableDetector detector;
    [SerializeField] private PlayerInteractionHandler handler;
    [SerializeField] private PlayerInteractionUI ui;

    public Iinteractable CurrentInteractable => detector?.GetCurrentInteractable();

    void Awake()
    {
        detector = GetComponent<PlayerInteractableDetector>();
        handler = GetComponent<PlayerInteractionHandler>();
        ui = GetComponent<PlayerInteractionUI>();
    }

    public bool CanHoldCurrentInteractable()
    {
        return handler?.CanHoldInteractWithCurrent() ?? false;
    }

    public void TryInteract()
    {
        handler?.TryInteract();
    }

    public void TryHoldInteract()
    {
        handler?.TryHoldInteract();
    }

    public void ClearInteractable()
    {
        detector?.ClearInteractable();
    }
}
