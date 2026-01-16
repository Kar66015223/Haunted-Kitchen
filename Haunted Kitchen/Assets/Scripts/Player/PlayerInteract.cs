using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;
    public float rayHeightOffset = -0.8f;

    public TMP_Text interactPrompt;

    private Iinteractable currentInteractable;

    private void Start()
    {
        interactPrompt.enabled = false;
    }

    private void Update()
    {
        CheckForInteractable();
    }

    public void TryInteract()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact(gameObject);
        }
    }

    public void CheckForInteractable()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeightOffset;
        Ray ray = new Ray(rayOrigin, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
        {
            Iinteractable interactable = hit.collider.GetComponent<Iinteractable>();

            if (interactable != null)
            {
                currentInteractable = interactable;
                interactPrompt.enabled = true;
            }
        }

        else
        {
            ClearInteractable(); 
        }
    }

    public void ClearInteractable()
    {
        currentInteractable = null;
        interactPrompt.enabled = false;
    }

#if UNITY_EDITOR // only show in editor
    void OnDrawGizmosSelected() // show interact range when selected
    {
        Gizmos.color = Color.yellow;
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeightOffset;
        Gizmos.DrawLine(rayOrigin, rayOrigin + transform.forward * interactRange);
    }
#endif
}
