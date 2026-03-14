using UnityEngine;

public class GhostLightFinder : MonoBehaviour
{
    public GameObject lightSwitch;
    [SerializeField] private PowerOutageSystem powerOutageSystem;

    private void Start()
    {
        lightSwitch = FindAnyObjectByType<LightSwitch>().gameObject;
        powerOutageSystem = FindAnyObjectByType<PowerOutageSystem>();
    }

    public void TurnOffLight()
    {
        LightSwitch Lswitch = lightSwitch.GetComponent<LightSwitch>();
        Lswitch.SetSwitchOff();
    }
}
