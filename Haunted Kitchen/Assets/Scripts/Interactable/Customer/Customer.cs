using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Customer : MonoBehaviour, Iinteractable, IContextInteractable
{
    public NavMeshAgent agent;

    public Table targetTable;
    public Transform standPoint;
    [SerializeField] private bool isArrived = false;
    [SerializeField] private bool exitDestinationSet = false;
    [SerializeField] private Transform exitPoint;

    public List<ItemData> possibleOrders = new();
    public ItemData orderedItem;

    public Image idleUI;
    public Image orderUI;

    [SerializeField] private CustomerState state = CustomerState.Idle;
    
    public enum CustomerState
    {
        Idle,
        Ordered,
        Served,
        Leaving
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (possibleOrders.Count == 0)
        {
            Debug.LogError("Customer has no possible orders!");
            return;
        }

        orderedItem = possibleOrders[Random.Range(0, possibleOrders.Count)];
        UpdateUI();
    }

    private void Update()
    {
        switch (state)
        {
            case CustomerState.Idle:
                HandleTableArrival();
                break;

            case CustomerState.Leaving:
                HandleLeaving(); 
                break;
        }
    }

    public void SetExitPoint(Transform exit)
    {
        exitPoint = exit;
    }

    void HandleTableArrival()
    {
        if (isArrived || agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            isArrived = true;
            agent.isStopped = true;

            transform.position = standPoint.position;
            transform.rotation = standPoint.rotation;

            UpdateUI();
        }
    }

    void HandleLeaving()
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
            Destroy(gameObject);
        }
    }

    public bool CanInteract(PlayerItem playerItem)
    {
        if (!isArrived) return false;

        switch (state)
        {
            case CustomerState.Idle:
                return true;

            case CustomerState.Ordered:
                if (playerItem == null) return false;
                if (playerItem.currentHeldItemObj == null) return false;

                return playerItem.currentHeldItemData is FoodData;

            case CustomerState.Served:
                return false;
        }

        return false;
    }

    public void Interact(GameObject interactor)
    {
        PlayerItem playerItem = interactor.GetComponent<PlayerItem>();

        switch (state)
        {
            case CustomerState.Idle:
                TakeOrder();
                break;

            case CustomerState.Ordered:
                ServeFood(playerItem);
                break;
        }
    }

    private void TakeOrder()
    {
        state = CustomerState.Ordered;
        UpdateUI();

        Debug.Log($"Customer ordered: {orderedItem}");
    }

    private void ServeFood(PlayerItem playerItem)
    {
        ItemData servedItem = playerItem.currentHeldItemData;
        bool correct = servedItem == orderedItem;

        if (correct)
        {
            Debug.Log("Correct order served!");
            // reward player
        }
        else
        {
            Debug.Log($"Wrong order! Expected {orderedItem.itemName}, got {servedItem.itemName}");
            // penalty / reaction
        }

        GameObject servedObj = playerItem.currentHeldItemObj;
        playerItem.DropItemNoRaycast();
        Destroy(servedObj);

        //state = CustomerState.Served; <= uncomment if customer have something else to do after served
        state = CustomerState.Leaving;
        isArrived = false;
        exitDestinationSet = false;

        UpdateUI();
    }

    public void UpdateUI()
    {
        idleUI.gameObject.SetActive(state == CustomerState.Idle && isArrived);

        orderUI.gameObject.SetActive(state == CustomerState.Ordered);

        if (state == CustomerState.Ordered && orderedItem != null)
        {
            orderUI.sprite = orderedItem.icon;
        }
    }
}
