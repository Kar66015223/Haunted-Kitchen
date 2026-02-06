using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GhostIdleState : IGhostState
{
    private GhostController controller;

    private float timer;
    private float changeDirTimer;

    private float idleDuration = 5f;
    private float directionChangeInterval = 1.5f;
    private float wanderSpeed = 2f;
    private float spawnRadius = 20f;
    private float wanderRadius = 6f;

    public GhostIdleState(GhostController controller)
    {
        this.controller = controller;
    }

    public void Enter()
    {
        //Teleport to random NaVMesh position
        if (NavMeshUtility.TryGetRandomPoint(
            controller.transform.position,
            spawnRadius,
            out Vector3 point))
        {
            controller.TeleportTo(point);
        }

        controller.movement.SetSpeed(wanderSpeed);

        timer = 0f;
        changeDirTimer = 0f;

        PickNewDestination();

        Debug.Log("Ghost enters Idle state");
    }

    public void Update()
    {
        timer += Time.deltaTime;
        changeDirTimer += Time.deltaTime;

        //Pick new destination periodically
        if (changeDirTimer >= directionChangeInterval)
        {
            PickNewDestination();
            changeDirTimer = 0f;
        }

        //Pick new destination when current destination reached
        if (!controller.agent.pathPending &&
            controller.agent.remainingDistance < 0.5f)
        {
            PickNewDestination();
        }

        //After idle duration, randomly switch state
        if (timer >= idleDuration)
        {
            controller.EnterRandomState();
            return;
        }

        //Debug.Log("Ghost is in idle state");
    }

    public void Exit()
    {
        controller.movement.Stop();

        Debug.Log("Ghost is exiting Idle state");
    }

    private void PickNewDestination()
    {
        if (NavMeshUtility.TryGetRandomPoint(
            controller.transform.position,
            wanderRadius,
            out Vector3 point))
        {
            controller.movement.MoveToward(point);
        }
    }
}
