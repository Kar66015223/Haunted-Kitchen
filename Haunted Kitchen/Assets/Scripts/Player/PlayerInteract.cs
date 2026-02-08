using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public TMP_Text interactPrompt;

    [SerializeField] private Iinteractable currentInteractable;
    private PlayerItem playerItem;

    private Outline currentOutline;

    public Iinteractable CurrentInteractable
    {
        get
        {
            return currentInteractable;
        }
    }

    private void Start()
    {
        interactPrompt.enabled = false;
        playerItem = GetComponent<PlayerItem>();
    }

    private void Update()
    {
        if (currentInteractable is IContextInteractable context)
        {
            if (!context.CanInteract(playerItem))
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

            if (obj.CompareTag("Item"))
            {
                Item item = obj.GetComponent<Item>();
                if (item != null)
                {
                    playerItem.PickUp(item.itemData, item.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Interactable") && !other.CompareTag("Item"))
            return;

        Iinteractable interactable = other.GetComponentInParent<Iinteractable>();
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
