using UnityEngine;

public class GhostIdleState : IGhostState
{
    private GhostController controller;

    public GhostIdleState(GhostController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        controller.movement.Stop();
    }

    public void Update()
    {
        Debug.Log("Ghost is in idle state");
    }

    public void Exit()
    {
        
    }
}
