using System;
using UnityEngine;

public class Oil : MonoBehaviour, Iinteractable, IWorkerInteractable
{
    public float slipDuration = 1f;
    public IWorkerTask Claimer { get; private set; }

    public event Action<IWorkerInteractable> OnFinished;

    void Start()
    {
        OnDiscovered();
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
            
            Clean();
        }
    }

    public void Clean()
    {
        OnFinished?.Invoke(this);
        Destroy(gameObject);
    }

    public bool TrySetClaimer(IWorkerTask claimer)
    {
        if (Claimer != null && Claimer != claimer)
            return false;

        Claimer = claimer;
        return true;
    }

    public void ClearClaimer(IWorkerTask claimer)
    {
        if (Claimer == claimer)
            Claimer = null;
    }

    public void OnDiscovered() => WorkerEvents.NotifyTaskDiscovered(this);

    public Transform GetPosition() => transform;
}
