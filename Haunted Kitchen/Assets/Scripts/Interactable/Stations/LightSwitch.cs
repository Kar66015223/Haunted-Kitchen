using Unity.VisualScripting;
using UnityEngine;

public class LightSwitch : MonoBehaviour, Iinteractable
{
    [SerializeField] private bool isOff = false;
    [SerializeField] private Light mainLight;

    private PowerOutageSystem outageSystem;

    void Start()
    {
        outageSystem = GameManager.instance.GetComponent<PowerOutageSystem>();
    }

    public bool CanInteract(Interactor interactor)
    {
        if(interactor == null) 
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        if (!isOff)
            return false;

        return true;
    }

    public void Interact(Interactor interactor)
    {
        Debug.Log("interacting with LightSwitch");
        if (mainLight == null)
        {
            Debug.LogError("Light is null");
        }

        SetSwitchOn();
    }

    public void SetSwitchOff()
    {
        isOff = true;
        outageSystem.TogglePower();
    }

    public void SetSwitchOn()
    {
        isOff = false;
        outageSystem.TogglePower();
    }
}
