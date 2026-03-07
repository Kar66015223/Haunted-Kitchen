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
    }

    public void Stop()
    {
        if (agent != null && agent.isOnNavMesh)
            agent.ResetPath();
    }

    public NavMeshAgent GetAgent() => agent;
}
