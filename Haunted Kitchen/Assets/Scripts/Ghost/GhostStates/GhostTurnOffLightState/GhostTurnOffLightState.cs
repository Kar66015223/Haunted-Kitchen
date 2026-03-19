using UnityEngine;

public class GhostTurnOffLightState : IGhostState
{
    private GhostController controller;
    private LightSwitch lightSwitch;

    public float turnOffDuration = 1f;
    private float timer;

    private bool actionPerformed = false;

    public GhostTurnOffLightState(GhostController controller, LightSwitch lightSwitch)
    {
        this.controller = controller;
        this.lightSwitch = lightSwitch;
    }

    public void Enter()
    {        
        controller.TeleportTo(lightSwitch.transform.position + lightSwitch.transform.forward * 1.5f);
        controller.transform.LookAt(lightSwitch.transform.position);
        controller.Movement.Stop();

        timer = 1f;
        actionPerformed = false;
    }
    public void Update()
    {
        if (controller.HitRuneStone)
        {
            Exit();
            return;
        }
        
        if (!actionPerformed)
        {
            lightSwitch.SetSwitchOff();
            actionPerformed = true;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Exit();
        }

        Debug.Log("Ghost is in TurnOffLight state");
    }

    public void Exit()
    {
        controller.Disappear();
        Debug.Log("Ghost is exiting TurnOffLight state");
    }
}
