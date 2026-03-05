using Unity.VisualScripting;
using UnityEngine;

public class GhostStateMachine
{
    private IGhostState currentState;

    public void ChangeState(IGhostState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
