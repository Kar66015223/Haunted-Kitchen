using UnityEngine;

public class OilInteraction : MonoBehaviour, Iinteractable, IinteractableVisual, ICleanable, IHoldInteractable
{
    [SerializeField] private Oil oil;
    [SerializeField] private MonoBehaviour visual;

    public float HoldThreshold => 1f;

    void Awake()
    {
        oil = GetComponentInParent<Oil>();
    }

    public MonoBehaviour GetVisualTarget() => visual;

    public bool CanInteract(Interactor interactor)
    {
        if (oil == null) return false;
        return oil.CanInteract(interactor);
    }

    public void Interact(Interactor interactor)
    {
        if (oil == null) return;
        oil.Interact(interactor);
    }
}
