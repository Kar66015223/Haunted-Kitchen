using System;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] private PlayerInteractableDetector detector;

    public static event Action<bool> OnHoldInteractValidityCheck;
    private bool validityState = false;

    void Awake()
    {
        detector = GetComponent<PlayerInteractableDetector>();
    }

    public bool CanInteractWithCurrent()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null)
            return false;

        var interactor = new Interactor(gameObject, InteractionType.Press);
        return interactable.CanInteract(interactor);
    }

    public bool CanHoldInteractWithCurrent()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null)
            return false;

        var mb = detector.GetCurrentInteractableMB();
        var interactor = new Interactor(gameObject, InteractionType.Hold);

        if (mb != null)
        {
            var table = mb.GetComponent<Table>();
            if (table != null)
                interactor.currentTable = table;
        }

        return interactable.CanInteract(interactor);
    }

    void Update()
    {
        bool currentValidity = CanHoldInteractWithCurrent();
        if(currentValidity != validityState)
        {
            validityState = currentValidity;
            OnHoldInteractValidityCheck?.Invoke(currentValidity);
        }
    }

    public void TryInteract()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null) return;

        var interactor = new Interactor(gameObject, InteractionType.Press);
        if (!interactable.CanInteract(interactor)) return;

        interactable.Interact(interactor);
    }

    public void TryHoldInteract()
    {
        var interactable = detector.GetCurrentInteractable();
        if (interactable == null) return;

        var mb = detector.GetCurrentInteractableMB();
        var interactor = new Interactor(gameObject, InteractionType.Hold);

        if (mb != null)
        {
            var table = mb.GetComponent<Table>();
            if (table != null)
                interactor.currentTable = table;
        }

        if (!interactable.CanInteract(interactor)) return;

        interactable.Interact(interactor);
    }
}
