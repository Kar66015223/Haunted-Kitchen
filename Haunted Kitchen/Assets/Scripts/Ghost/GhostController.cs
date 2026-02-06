using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    public Transform player;
    public GhostMovement movement;

    private GhostStateMachine stateMachine;

    public NavMeshAgent agent { get; private set; }

    public System.Action OnGhostDestroyed;

    private void Awake()
    {
        stateMachine = new GhostStateMachine();

        movement = GetComponent<GhostMovement>();
        agent = GetComponent<NavMeshAgent>();

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

    public void EnterRandomState()
    {
        GhostSpawner.GhostStartBehavior next;

        //Prevent ghost from entering Idle again
        do
        {
            int count = System.Enum.GetValues(typeof(GhostSpawner.GhostStartBehavior)).Length;
            next = (GhostSpawner.GhostStartBehavior)Random.Range(0, count);
        }
        while (next == GhostSpawner.GhostStartBehavior.Idle);

        IGhostState state = next switch
        {
            GhostSpawner.GhostStartBehavior.PourOil =>
                new GhostPourOilState(this),

            //GhostSpawner.GhostStartBehavior.TurnOffLight =>
            //    new GhostTurnOffLightState(this),

            _ =>
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
        OnGhostDestroyed?.Invoke();
        Destroy(gameObject);
    }
}
