using UnityEngine;

public class GhostTurnOffLightState : IGhostState
{
    private GhostController controller;
    private GhostLightFinder lightFinder;

    public float turnOffDuration = 1f;
    private float timer;

    public GhostTurnOffLightState(GhostController controller)
    {
        this.controller = controller;
        lightFinder = controller.GetComponent<GhostLightFinder>();
    }

    public void Enter()
    {
        if (lightFinder != null)
        {
            Vector3 offset = lightFinder.lightSwitch.transform.forward * 1.5f;
            Vector3 teleportPosition = lightFinder.lightSwitch.transform.position + offset;

            controller.TeleportTo(teleportPosition);
            controller.transform.LookAt(lightFinder.lightSwitch.transform.position);

            lightFinder.TurnOffLight();
        }

        timer = turnOffDuration;

        Debug.Log("Ghost enters TurnOffLight state");
    }
    public void Update()
    {
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
