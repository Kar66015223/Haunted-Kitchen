using System;
using UnityEngine;

public class Oil : MonoBehaviour, Iinteractable
{
    public float slipDuration = 1f;

    void Start()
    {
        GameEvents.OnOilSpawned?.Invoke(gameObject);
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
            Destroy(gameObject);
        }
    }
}
