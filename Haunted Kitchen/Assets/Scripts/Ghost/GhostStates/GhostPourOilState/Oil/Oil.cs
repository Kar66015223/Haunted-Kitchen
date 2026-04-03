using System;
using UnityEngine;

public class Oil : MonoBehaviour, Iinteractable, IWorkerInteractable
{
    public float slipDuration = 1f;

    [SerializeField] private bool _isTarget = false;
    public bool IsTargeted { get => _isTarget; set => _isTarget = value; }

    void Start()
    {
        WorkerEvents.NotifyTaskDiscovered(this);
    }

    public bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
            return false;

        var playerItem = interactor.playerItem;

        if (interactor.interactionType == InteractionType.Hold)
        {
            if (playerItem == null)
                return false;

            if (playerItem.currentHeldItemObj != null)
                return false;

            return true;
        }

        return false;
    }


    public void Interact(Interactor interactor)
    {
        Debug.Log("Oil interaction successfull");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController_New controller = other.GetComponent<PlayerController_New>();

            if (controller != null)
            {
                controller.Slip(slipDuration);
            }

            Debug.Log($"{other.gameObject.name} stepped on an oil");
            Clean();
        }
    }

    public void Clean()
    {
        Destroy(gameObject);
    }

    public void OnDiscovered() { }

    public Transform GetPosition() => transform;
}
