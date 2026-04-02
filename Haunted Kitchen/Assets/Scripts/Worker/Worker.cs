using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private WorkerContext context;
    private List<IWorkerTask> availableTask = new();
    private IWorkerTask currentTask;
    private WorkerState currentState = WorkerState.Idle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        context = new WorkerContext { Agent = agent, StoppingDistance = agent.stoppingDistance };

        RegisterTask();
    }

    void RegisterTask()
    {
        // availableTask.Add(new CleanOilTask());
    }

    void Update()
    {
        switch (currentState)
        {
            case WorkerState.Idle:
                UpdateIdle();
                break;
            case WorkerState.MovingToTarget:
                UpdateMoving();
                break;
            case WorkerState.Executing:
                UpdateExecuting();
                break;
        }
    }

    void UpdateIdle()
    {
        var nextTask = availableTask
            .Where(t => t.CanExecute(context))
            .OrderByDescending(t => t.Priority)
            .FirstOrDefault();

        if (nextTask != null && nextTask != currentTask)
        {
            SwitchTask(nextTask);
        }
    }

    void UpdateMoving()
    {
        currentTask?.Update(context);

        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentState = WorkerState.Executing;
        }
    }

    void UpdateExecuting()
    {
        currentTask?.Update(context);

        if (currentTask?.IsComplete(context) ?? true)
        {
            currentTask?.End(context);
            currentTask = null;
            currentState = WorkerState.Idle;
        }
    }
    
    void SwitchTask(IWorkerTask newTask)
    {
        currentTask?.End(context);
        currentTask = newTask;
        currentTask.Start(context);
        currentState = WorkerState.MovingToTarget;
    }
}
