using UnityEngine;
using UnityEngine.AI;

public class WorkerContext
{
    public Transform CurrentTarget { get; set; }
    public NavMeshAgent Agent { get; set; }
    public float StoppingDistance { get; set; }
}
