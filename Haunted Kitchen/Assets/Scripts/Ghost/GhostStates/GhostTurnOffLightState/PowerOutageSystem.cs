using UnityEngine;

public class PowerOutageSystem : MonoBehaviour
{
    [SerializeField] private GameObject powerOutageObj;
    [SerializeField] private bool isOff = false;
    public bool IsOff => isOff;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TogglePower();
        }
    }

    public void TogglePower()
    {
        isOff = !isOff;
        if (powerOutageObj != null)
        {
            powerOutageObj.SetActive(isOff);
            GameEvents.OnLightOut?.Invoke(isOff);
        }
    }
}