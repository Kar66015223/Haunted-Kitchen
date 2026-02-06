using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GhostMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GhostController controller;

    private void Awake()
    {
        controller = GetComponent<GhostController>();
    }

    public void SetSpeed(float value)
    {
        controller.agent.speed = value;
    }

    public void MoveToward(Vector3 target)
    {
        controller.agent.SetDestination(target);
    }

    public void Stop()
    {
        controller.agent.ResetPath();
    }
}
