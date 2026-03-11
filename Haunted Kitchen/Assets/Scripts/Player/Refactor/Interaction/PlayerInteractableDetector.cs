using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;

public class PlayerInteractableDetector : MonoBehaviour
{
    // Key to check if it's detected and find closest, Value to calculate distance between objects
    private Dictionary<Iinteractable, MonoBehaviour> detectedInteractables = new();
    [SerializeField] private MonoBehaviour currentInteractableMB;
    [SerializeField] private Iinteractable currentInteractable;

    public event Action<Iinteractable> OnInteractableDetected;
    public event Action OnInteractableLost;

    private void OnTriggerStay(Collider other)
    {
        var interactable = other.GetComponentInParent<Iinteractable>();
        if (interactable == null) return;

        var mb = other.GetComponentInParent<MonoBehaviour>();
        if (mb == null) return;

        if (!detectedInteractables.ContainsKey(interactable))
        {
            ClearInteractable();

            // Storing mb to kvp.Value
            detectedInteractables[interactable] = mb;
        }

        UpdateClosestInteractable();
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<Iinteractable>();
        if (interactable == null) return;

        detectedInteractables.Remove(interactable);

        UpdateClosestInteractable();
    }

    private void UpdateClosestInteractable()
    {
        CleanupInactiveColliders();

        if (detectedInteractables.Count == 0)
        {
            if (currentInteractable != null)
            {
                ClearInteractable();
            }

            return;
        }

        // Check distance using kvp.Value
        var closest = detectedInteractables
            .OrderBy(kvp =>
            {
                float distance = Vector3.Distance(transform.position, kvp.Value.transform.position);

                // Prevent equal distance
                if (kvp.Key == currentInteractable)
                {
                    distance -= 0.01f;
                }
                
                return distance;
            })
            .First();

        if(currentInteractable != closest.Key)
        {
            currentInteractable = closest.Key;
            currentInteractableMB = closest.Value;
            OnInteractableDetected?.Invoke(closest.Key);
        }
    }

    private void CleanupInactiveColliders()
    {
        var toRemove = new List<Iinteractable>();

        foreach (var kvp in detectedInteractables)
        {
            var mb = kvp.Value;
            if (mb == null)
            {
                toRemove.Add(kvp.Key);
                continue;
            }

            var colliders = mb.GetComponentsInChildren<Collider>();

            bool hasActiveCollider = colliders.Any(c =>
                c.enabled && c.gameObject.activeInHierarchy);

            if (!hasActiveCollider)
            {
                toRemove.Add(kvp.Key);
            }
        }
        
        // Remove inactive interactables
        foreach(var interactable in toRemove)
        {
            bool WasCurrentInteractable = interactable == currentInteractable;
            detectedInteractables.Remove(interactable);

            // If removed item was current, update to new closest
            if(WasCurrentInteractable)
            {
                ClearInteractable();
            }
        }
    }

    public void ClearInteractable()
    {
        if (currentInteractable != null)
        {
            OnInteractableLost?.Invoke();
        }

        currentInteractable = null;
        currentInteractableMB = null;

        detectedInteractables.Clear();
    }
    
    public Iinteractable GetCurrentInteractable() => currentInteractable;
    public MonoBehaviour GetCurrentInteractableMB() => currentInteractableMB;
}
