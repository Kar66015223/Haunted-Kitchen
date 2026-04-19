using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class CustomerMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator anim;

    [Header("Standpoint")]
    [SerializeField] private List<Table> tables = new();
    [SerializeField] private Table targetTable;
    [SerializeField] private Chair targetChair;
    [SerializeField] private Transform standPoint;

    [Header("Arriving")]
    [SerializeField] private bool isArrived = false;
    public bool IsArrived => isArrived;
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

    private void Update()
    {
        HandleTableArrival();
    }

    public void Initialize(List<Table> availableTables, Transform exit)
    {
        tables = availableTables;
        exitPoint = exit;

        if (tables.Count == 0)
        {
            Debug.LogError("No tables assigned in inspector");
            return;
        }

        //pick random free table
        List<Table> freeTables = tables.FindAll(tables => tables.HasFreeChair());

        if (freeTables.Count == 0)
        {
            Debug.Log("No free tables");
            return;
        }

        targetTable = freeTables[UnityEngine.Random.Range(0, freeTables.Count)];
        targetChair = targetTable.GetFreeChair();

        standPoint = targetChair.CustomerStandPoint;
        targetChair.isOccupied = true;

        agent.SetDestination(standPoint.position);
    }

    public void HandleTableArrival()
    {
        if (standPoint == null) return;

        if (isArrived || agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isArrived = true;
            agent.isStopped = true;

            agent.enabled = false;

            transform.SetPositionAndRotation(standPoint.position, standPoint.rotation);
            SitOnChair();

            OnArrived?.Invoke();
            GameEvents.OnCustomerArrived?.Invoke(GetComponent<Customer_New>());

            anim.SetBool(CustomerConstants.ANIM_SIT, true);
        }
    }
    
    public void SitOnChair()
    {
        if (targetChair != null)
            {
                var customer = GetComponent<Customer_New>();
                customer.customerGraphic.SetActive(false);
                
                if(customer.TryGetComponent(out Collider col))
                {
                    col.enabled = false;
                }

                targetChair.SetCurrentCustomer(customer);
            }
    }

    public void HandleLeaving()
    {
        if (!exitDestinationSet)
        {
            LeaveChair();

            agent.enabled = true;

            agent.isStopped = false;
            agent.SetDestination(exitPoint.position);
            exitDestinationSet = true;
        }

        //arrived at exitPoint
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // targetChair.isOccupied = false;
            OnLeft?.Invoke();
            OnLeft = null;

            Destroy(gameObject);
        }

        anim.SetBool(CustomerConstants.ANIM_SIT, false);
    }

    public void LeaveChair()
    {
        if (targetChair != null)
        {
            var customer = GetComponent<Customer_New>();
            customer.customerGraphic.SetActive(true);

            if (customer.TryGetComponent(out Collider col))
            {
                col.enabled = true;
            }

            targetChair.ClearCustomer();
        }
    }

    public Table GetCurrentTable() => targetTable;

    public void PlayAttack()    
    {
        if (anim != null)
            anim.SetTrigger(CustomerConstants.ANIM_ATTACK);
    }

    public void PlayIdle()
    {
        if (anim != null)
            anim.SetBool(CustomerConstants.ANIM_SIT, false);
    }

    public void PlaySit()
    {
        if (anim != null)
            anim.SetBool(CustomerConstants.ANIM_SIT, true);
    }
}
