using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using Unity.VisualScripting;

public class GhostController : MonoBehaviour
{
    private INPCMovementController movement;
    private IAnimationController anim;
    private GhostStateMachine stateMachine;
    private GhostStateFactory stateFactory;

    public Transform player { get; private set; }
    private LightSwitch lightSwitch;
    private List<IDestroyable> destroyTargets;
    [SerializeField] private GameObject oilPrefab;
    [SerializeField] private GameObject oilCup;

    public NavMeshAgent agent { get; private set; }

    public INPCMovementController Movement => movement;
    public IAnimationController Anim => anim;

    private bool hitRuneStone = false;
    public bool HitRuneStone => hitRuneStone;

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

        FindDependencies();
    }
    
    private void FindDependencies()
    {
        lightSwitch = FindAnyObjectByType<LightSwitch>();
        if (lightSwitch == null)
            Debug.LogWarning("LightSwitch not found in scene");

        destroyTargets = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDestroyable>()
            .ToList();
        if (destroyTargets.Count == 0)
            Debug.LogWarning("No IDestroyables found in scene");
    }

    private void Update()
    {
        stateMachine?.Update();
    }

    public void SetInitialState(GhostStartBehavior behavior)
    {
        var state = stateFactory.CreateState(behavior, this, lightSwitch, destroyTargets, oilPrefab, oilCup);
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

        var state = stateFactory.CreateState(randomBehavior, this, lightSwitch, destroyTargets, oilPrefab, oilCup);

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

    public void HitRune()
    {
        hitRuneStone = true;
        Debug.Log("Ghost hit rune stone");
    }

    public LightSwitch GetLightSwitch() => lightSwitch;
    public List<IDestroyable> GetDestroyTargets() => destroyTargets;
    public GameObject GetOilPrefab() => oilPrefab;
}
