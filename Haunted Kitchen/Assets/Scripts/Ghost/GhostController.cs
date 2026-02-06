using UnityEngine;

public class GhostController : MonoBehaviour
{
    public Transform player;
    public GhostMovement movement;

    private GhostStateMachine stateMachine;

    private void Awake()
    {
        stateMachine = new GhostStateMachine();
        movement = GetComponent<GhostMovement>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public void SetInitialState(GhostSpawner.GhostStartBehavior behavior)
    {
        IGhostState state = behavior switch
        {
            //GhostSpawner.GhostStartBehavior.PourOil =>
            //    new GhostPourOilState(this),

            //GhostSpawner.GhostStartBehavior.TurnOffLight =>
            //    new GhostTurnOffLightState(this),

            _ => //same as "default:"
                new GhostIdleState(this)
        };

        ChangeState(state);
    }

    public void ChangeState(IGhostState newState)
    {
        stateMachine.ChangeState(newState);
    }

    public void TeleportTo(Vector3 position)
    {
        transform.position = position;
    }

    public void Disappear()
    {
        Destroy(gameObject);
    }
}
