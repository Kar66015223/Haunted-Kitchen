using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public TMP_Text interactPrompt;

    [SerializeField] private Iinteractable currentInteractable;
    public Iinteractable CurrentInteractable => currentInteractable;

    [SerializeField] private IHoldInteractable currentHoldInteractable;
    public IHoldInteractable CurrentHoldInteractable => currentHoldInteractable;

    private PlayerItem playerItem;
    private PlayerController controller;

    private Outline currentOutline;

    private void Start()
    {
        interactPrompt.enabled = false;
        playerItem = GetComponent<PlayerItem>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (currentInteractable is IContextInteractable context)
        {
            if (!context.CanInteract(playerItem) || controller.IsSlipping)
            {
                ClearInteractable();
            }
        }
    }

    public void TryInteract()
    {
        if (currentInteractable == null) return;

        currentInteractable.Interact(gameObject);

        // PickUp
        if (playerItem.currentHeldItemData == null) // If player's hands is free
        {
            GameObject obj = ((MonoBehaviour)currentInteractable).gameObject; // Iinteractable with Monobehaviour
            Item item = obj.GetComponent<Item>();

            if (/*obj.CompareTag("Item")*/item != null)
            {
                playerItem.PickUp(item.itemData, item.gameObject);
            }
        }
    }

    public void TryHoldInteract()
    {
        if (currentInteractable == null) return;

        currentHoldInteractable?.HoldInteract(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        //if (!other.CompareTag("Interactable") && !other.CompareTag("Item"))
        //    return;

        Iinteractable interactable = other.GetComponentInParent<Iinteractable>();
        currentHoldInteractable = interactable as IHoldInteractable;
        if (interactable == null) return;

        if (interactable is IContextInteractable context)
        {
            if (!context.CanInteract(playerItem))
                return;
        }

        if (currentInteractable != interactable)
        {
            ClearOutline();

            currentInteractable = interactable;
            interactPrompt.enabled = true;

            SetOutline(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Iinteractable interactable = other.GetComponentInParent<Iinteractable>();

        if(interactable == null) return;

        if (interactable == currentInteractable)
        {
            ClearInteractable();
        }
    }

    public void ClearInteractable()
    {
        currentInteractable = null;
        currentHoldInteractable = null;
        interactPrompt.enabled = false;

        ClearOutline();
    }

    private void SetOutline(Iinteractable interactable)
    {
        Outline outline = ((MonoBehaviour)interactable).gameObject.GetComponentInChildren<Outline>();

        if (outline == null)
        {
            Debug.LogError("No outline object found");
            return;
        }

        currentOutline = outline;

        currentOutline.enabled = true;
        currentOutline.OutlineColor = Color.white;
    }

    private void ClearOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
}
