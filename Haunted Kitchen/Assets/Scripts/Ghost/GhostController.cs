using System;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    private INPCMovementController movement;
    private IAnimationController anim;
    private GhostStateMachine stateMachine;
    private GhostStateFactory stateFactory;

    public Transform player {get; private set;}
    public NavMeshAgent agent { get; private set; }

    public INPCMovementController Movement => movement;
    public IAnimationController Anim => anim;

    public event Action OnGhostDestroyed;

    private void Awake()
    {
        stateMachine = new GhostStateMachine();
        stateFactory = new GhostStateFactory();

        var movementComponent = GetComponent<GhostMovement>();
        var animationComponent = GetComponent<GhostAnimation>();

        movement = movementComponent ?? throw new System.NullReferenceException("GhostMovementController required");
        anim = animationComponent ?? throw new System.NullReferenceException("GhostAnimationController required");
        agent = movementComponent?.GetAgent();

        var playerGO = GameObject.FindGameObjectWithTag(GhostConstants.PLAYER_TAG);
        player = playerGO?.transform;

        if (player == null)
            Debug.LogWarning($"Player with tag '{GhostConstants.PLAYER_TAG}' not found");
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public void SetInitialState(GhostStartBehavior behavior)
    {
        var state = stateFactory.CreateState(behavior, this);

        ChangeState(state);
    }

    public void EnterRandomState()
    {
        GhostStartBehavior randomBehavior;

        //Prevent ghost from entering Idle again
        do
        {
            int count = System.Enum.GetValues(typeof(GhostStartBehavior)).Length;
            randomBehavior = (GhostStartBehavior)UnityEngine.Random.Range(0, count);
        }
        while (randomBehavior == GhostStartBehavior.Idle);

        var state = stateFactory.CreateState(randomBehavior, this);

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
