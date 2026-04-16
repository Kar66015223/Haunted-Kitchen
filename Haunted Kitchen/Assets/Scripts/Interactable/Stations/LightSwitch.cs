using UnityEngine;

public class LightSwitch : MonoBehaviour, Iinteractable
{
    private PowerOutageSystem outageSystem;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        outageSystem = FindAnyObjectByType<PowerOutageSystem>();
    }

    void Update()
    {
        if (outageSystem == null) return;

        if (!outageSystem.IsOff) return;

        if (outageSystem.IsOff)
            GetComponent<Outline>().enabled = true;
    }

    public bool CanInteract(Interactor interactor)
    {
        if(interactor == null) 
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        if (outageSystem != null && !outageSystem.IsOff)
            return false;

        return true;
    }

    public void Interact(Interactor interactor)
    {
        Debug.Log("interacting with LightSwitch");
        SetSwitchOn();
    }

    public void SetSwitchOn()
    {
        if (!outageSystem.IsOff) return;
        outageSystem.TogglePower();
        audioSource.Play();
    }

    public void SetSwitchOff()
    {
        if (outageSystem.IsOff) return;
        outageSystem.TogglePower();
    }
}
