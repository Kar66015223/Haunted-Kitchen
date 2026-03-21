using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public TMP_Text interactPrompt;

    [SerializeField] private MonoBehaviour currentInteractableMB;
    [SerializeField] private Iinteractable currentInteractable;
    public Iinteractable CurrentInteractable => currentInteractable;

    private PlayerItem playerItem;
    private PlayerController controller;
    private PlayerPossession possession;

    private Outline currentOutline;

    private void Start()
    {
        interactPrompt.enabled = false;
        playerItem = GetComponent<PlayerItem>();
        controller = GetComponent<PlayerController>();
        possession = GetComponent<PlayerPossession>();
    }

    private void Update()
    {
        if (currentInteractable != null && 
            controller != null &&
            playerItem != null)
        {
            if (!currentInteractable.CanInteract(new Interactor(gameObject, InteractionType.Press)))
            {
                ClearInteractable();
            }

            if (controller.IsSlipping || possession.IsPossessed)
            {
                ClearInteractable();
            }
        }
    }

    public bool CanHoldCurrentInteractable()
    {
        if (currentInteractable == null)
            return false;

        var interactor = new Interactor(gameObject, InteractionType.Hold);

        if (currentInteractableMB != null)
        {
            var table = currentInteractableMB.GetComponent<Table>();
            if(table != null)
                interactor.currentTable = table;
        }

        return currentInteractable.CanInteract(interactor);
    }

    public void TryInteract()
    {
        if (currentInteractable == null) return;

        var interactor = new Interactor(gameObject, InteractionType.Press);
        if (!currentInteractable.CanInteract(interactor)) return;

        currentInteractable.Interact(interactor);

        if (playerItem.currentHeldItemData == null && currentInteractableMB != null)
        {
            var mbItem = currentInteractableMB.GetComponent<Item>();
            var mbTable = currentInteractableMB.GetComponent<Table>();

            if (mbItem != null && mbTable == null)
            {
                if (mbItem.itemState != ItemState.Held && playerItem != null)
                {
                    playerItem.PickUp(mbItem.itemData, mbItem.gameObject);
                }
            }
        }
    }

    public void TryHoldInteract()
    {
        if (currentInteractable == null) return;

        var interactor = new Interactor(gameObject, InteractionType.Hold);

        if (currentInteractable != null)
        {
            var table = currentInteractableMB.GetComponent<Table>();
            if(table != null) interactor.currentTable = table;
        }

        if (!currentInteractable.CanInteract(interactor)) return;
        currentInteractable.Interact(interactor);
    }

    private void OnTriggerStay(Collider other)
    {
        var interactable = other.GetComponentInParent<Iinteractable>();
        if (interactable == null) return;

        var mb = other.GetComponentInParent<MonoBehaviour>();

        var interactor = new Interactor(gameObject, InteractionType.Press);
        if (!interactable.CanInteract(interactor)) return;

        if (currentInteractable != interactable)
        {
            ClearOutline();

            currentInteractable = interactable;
            currentInteractableMB = mb;
            interactPrompt.enabled = true;

            SetOutline(mb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<Iinteractable>();
        if(interactable == null) return;

        if (interactable == currentInteractable)
        {
            ClearInteractable();
        }
    }

    public void ClearInteractable()
    {
        currentInteractable = null;
        currentInteractableMB = null;

        interactPrompt.enabled = false;

        ClearOutline();
    }

    private void SetOutline(MonoBehaviour interactableMB)
    {
        if(interactableMB == null) return;
        var outline = interactableMB.gameObject.GetComponentInChildren<Outline>();

        if (outline == null)
        {
            Debug.LogWarning($"No outline object found in {interactableMB.gameObject.name}");
            return;
        }
            
        currentOutline = outline;
        currentOutline.enabled = true;
        //currentOutline.OutlineColor = Color.white;
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
