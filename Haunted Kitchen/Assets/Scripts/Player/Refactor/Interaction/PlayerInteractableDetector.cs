using UnityEngine;
using System;

public class PlayerInteractableDetector : MonoBehaviour
{
    [SerializeField] private MonoBehaviour currentInteractableMB;
    [SerializeField] private Iinteractable currentInteractable;

    public event Action<Iinteractable> OnInteractableDetected;
    public event Action OnInteractableLost;

    private void OnTriggerStay(Collider other)
    {
        var interactable = other.GetComponentInParent<Iinteractable>();
        if (interactable == null) return;

        if (currentInteractable != interactable)
        {
            currentInteractable = interactable;
            currentInteractableMB = other.GetComponentInParent<MonoBehaviour>();
            OnInteractableDetected?.Invoke(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<Iinteractable>();
        if (interactable == currentInteractable)
        {
            ClearInteractable();
        }
    }

    public void ClearInteractable()
    {
        if (currentInteractable != null)
            OnInteractableLost?.Invoke();

        currentInteractable = null;
        currentInteractableMB = null;
    }
    
    public Iinteractable GetCurrentInteractable() => currentInteractable;
    public MonoBehaviour GetCurrentInteractableMB() => currentInteractableMB;
}
