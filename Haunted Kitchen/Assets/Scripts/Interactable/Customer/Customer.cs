using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Security.Cryptography;

public class Customer : MonoBehaviour, Iinteractable, IContextInteractable
{
    public NavMeshAgent agent;

    [Header("Tables")]
    public Table targetTable;
    public Transform standPoint;
    [SerializeField] private bool isArrived = false;
    [SerializeField] private bool exitDestinationSet = false;
    [SerializeField] private Transform exitPoint;

    public System.Action OnCustomerLeft;

    [Header("Order")]
    public List<ItemData> possibleOrders = new();
    public ItemData orderedItem;

    public Image idleUI;
    public Image orderUI;

    [Header("Money Steal")]
    private int stealAmount;

    [Header("Patience")]
    [SerializeField] private Image patienceImg;
    [SerializeField] private float patienceDuration = 60f;

    private float patienceTimer;
    private bool isCountingPatience = false;

    [SerializeField] private CustomerState state = CustomerState.Idle;

    public Animator anim;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
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

        if (isArrived && state != CustomerState.Leaving)
        {
            HandlePatience();
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

            StartPatienceTimer();

            UpdateUI();

            anim.SetBool("Sit", true);
        }
    }

    void StartPatienceTimer()
    {
        patienceTimer = patienceDuration;
        isCountingPatience = true;

        patienceImg.fillAmount = 1f;
    }

    void HandlePatience()
    {
        if (!isCountingPatience) return;

        patienceTimer -= Time.deltaTime;

        float normalized = patienceTimer / patienceDuration;
        patienceImg.fillAmount = normalized;

        if (patienceTimer <= 0f)
        {
            isCountingPatience = false;

            stealAmount = Random.Range(300, 1000);
            GameManager.instance.playerMoney.ChangeMoneyAmount(-stealAmount);
            GameManager.instance.ShowEventText("Your money was stolen by an angry customer...", Color.red);

            state = CustomerState.Leaving;
            isArrived = false;
            exitDestinationSet = false;

            UpdateUI();

            Debug.Log("Customer lost patience.");
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
            targetTable.isOccupied = false;
            OnCustomerLeft?.Invoke();
            OnCustomerLeft = null;

            Destroy(gameObject);
        }

        anim.SetBool("Sit", false);
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

        StartPatienceTimer();
        UpdateUI();

        Debug.Log($"Customer ordered: {orderedItem}");
    }

    private void ServeFood(PlayerItem playerItem)
    {
        ItemData servedItem = playerItem.currentHeldItemData;
        bool correct = servedItem == orderedItem;

        isCountingPatience = false;

        if (correct)
        {
            Debug.Log("Correct order served!");

            GameManager.instance.playerMoney.ChangeMoneyAmount(servedItem.price);
        }
        else
        {
            stealAmount = Random.Range(300, 1000);

            GameManager.instance.playerMoney.ChangeMoneyAmount(-stealAmount);
            GameManager.instance.ShowEventText("Your money was stolen by an angry customer...", Color.red);
        }

        GameObject servedObj = playerItem.currentHeldItemObj;
        playerItem.DropItemNoRaycast();
        Destroy(servedObj);

        state = CustomerState.Leaving;
        isArrived = false;
        exitDestinationSet = false;

        UpdateUI();
    }

    public void UpdateUI()
    {
        idleUI.gameObject.SetActive(state == CustomerState.Idle && isArrived);

        orderUI.gameObject.SetActive(state == CustomerState.Ordered);

        patienceImg.gameObject.SetActive(isArrived && state != CustomerState.Leaving);

        if (state == CustomerState.Ordered && orderedItem != null)
        {
            orderUI.sprite = orderedItem.icon;
        }
    }
}
