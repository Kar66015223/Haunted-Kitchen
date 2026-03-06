using UnityEngine;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour, IMovementController
{
    [SerializeField] private float speed;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetSpeed(float value)
    {
        if(agent != null)
            agent.speed = value;
    }

    public void MoveToward(Vector3 target)
    {
        if (agent != null && agent.isOnNavMesh)
            agent.SetDestination(target);

        // controller.anim.SetInteger(GhostConstants.ANIM_STATE, 1);
    }

    public void Stop()
    {
        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();

        // controller.anim.SetInteger(GhostConstants.ANIM_STATE, 0);
    }

    public NavMeshAgent GetAgent() => agent;
}
