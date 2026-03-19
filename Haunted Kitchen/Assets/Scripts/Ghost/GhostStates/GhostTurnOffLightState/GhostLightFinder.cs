using UnityEngine;

public class GhostLightFinder : MonoBehaviour
{
    public GameObject lightSwitch;

    private void Start()
    {
        lightSwitch = FindAnyObjectByType<LightSwitch>().gameObject;
    }

    public void TurnOffLight()
    {
        LightSwitch Lswitch = lightSwitch.GetComponent<LightSwitch>();
        Lswitch.SetSwitchOff();
    }
}
