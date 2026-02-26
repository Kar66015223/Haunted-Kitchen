using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    [Header("Standpoint")]
    [SerializeField] private List<Table> tables = new();
    public Table targetTable;
    public Transform standPoint;

    [Header("Arriving")]
    [SerializeField] private bool isArrived = false;
    public bool IsArrived { get { return isArrived; } }
    public event Action OnArrived;

    [Header("Leaving")]
    [SerializeField] private bool exitDestinationSet = false;
    [SerializeField] private Transform exitPoint;
    public Action OnLeft;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (tables.Count == 0)
        {
            Debug.LogError("No tables assigned in inspector");
            return;
        }

        //pick random free table
        List<Table> freeTables = tables.FindAll(tables => !tables.isOccupied);

        if (freeTables.Count == 0)
        {
            Debug.Log("No free tables");
            return;
        }

        targetTable = freeTables[UnityEngine.Random.Range(0, freeTables.Count)];
        standPoint = targetTable.customerStandPoint;
        targetTable.isOccupied = true;

        agent.SetDestination(standPoint.position);
    }

    private void Update()
    {
        HandleTableArrival();
    }

    public void Initialize(List<Table> availableTables, Transform exit)
    {
        tables = availableTables;
        exitPoint = exit;
    }

    public void HandleTableArrival()
    {
        if (isArrived || agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isArrived = true;
            agent.isStopped = true;

            transform.position = standPoint.position;
            transform.rotation = standPoint.rotation;

            OnArrived?.Invoke();

            //UpdateUI();

            anim.SetBool("Sit", true);
        }
    }

    public void HandleLeaving()
    {
        if (!exitDestinationSet)
        {
            agent.isStopped = false;
            agent.SetDestination(exitPoint.position);
            exitDestinationSet = true;
        }

        //arrived at exitPoint
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            targetTable.isOccupied = false;
            OnLeft?.Invoke();
            OnLeft = null;

            Destroy(gameObject);
        }

        anim.SetBool("Sit", false);
    }
}
