using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;
    public float rayHeightOffset = -0.8f;

    public TMP_Text interactPrompt;

    private Iinteractable currentInteractable;

    public void TryInteract()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * rayHeightOffset;
        Ray ray = new Ray(rayOrigin, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            Iinteractable obj = hit.collider.GetComponent<Iinteractable>();

            if (obj != null)
            {
                obj.Interact(gameObject);
            }
        }
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
