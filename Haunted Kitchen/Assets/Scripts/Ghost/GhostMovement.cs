using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetSpeed(float value)
    {
        agent.speed = value;
    }

    public void MoveToward(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void Stop()
    {
        agent.ResetPath();
    }
}
