using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    public Transform player;
    public GhostMovement movement;

    private GhostStateMachine stateMachine;

    public NavMeshAgent agent { get; private set; }

    public System.Action OnGhostDestroyed;

    public Animator anim;

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

    public void SetInitialState(GhostStartBehavior behavior)
    {
        IGhostState state = behavior switch
        {
            _ => //same as "default:"
                new GhostIdleState(this)
        };

        ChangeState(state);
    }

    public void EnterRandomState()
    {
        GhostStartBehavior next;

        //Prevent ghost from entering Idle again
        do
        {
            int count = System.Enum.GetValues(typeof(GhostStartBehavior)).Length;
            next = (GhostStartBehavior)Random.Range(0, count);
        }
        while (next == GhostStartBehavior.Idle);

        IGhostState state = next switch
        {
            GhostStartBehavior.PourOil =>
                new GhostPourOilState(this),

            GhostStartBehavior.TurnOffLight =>
                new GhostTurnOffLightState(this),

            GhostStartBehavior.Destroy =>
                new GhostDestroyState(this),

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
