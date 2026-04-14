using UnityEngine;
using System;

public class Customer_New : MonoBehaviour, Iinteractable, IWorkerInteractable
{
    public CustomerMovement movement;
    public CustomerPatience patience;
    public CustomerOrder orderSystem;
    public CustomerUI ui;
    public CustomerBehavior behavior;

    public GameObject customerGraphic;
    [SerializeField] private CustomerState state = CustomerState.Idle;

    public IWorkerTask Claimer { get; private set; }
    public IWorkerTask claimer => Claimer;

    public event Action<CustomerState> OnStateChanged;
    public event Action<IWorkerInteractable> OnFinished;
    public event Action<IWorkerInteractable> OnOrderTaken;

    private void OnEnable()
    {
        movement.OnArrived += HandleArrival;
        patience.OnPatienceExpired += HandlePatienceExpired;
        orderSystem.OnOrderServed += HandleOrder;
    }
    private void OnDisable()
    {
        movement.OnArrived -= HandleArrival;
        patience.OnPatienceExpired -= HandlePatienceExpired;
        orderSystem.OnOrderServed -= HandleOrder;
    }

    private void Awake()
    {
        movement = GetComponent<CustomerMovement>();
        patience = GetComponent<CustomerPatience>();
        orderSystem = GetComponent<CustomerOrder>();
        ui = GetComponent<CustomerUI>();
        behavior = GetComponent<CustomerBehavior>();
        customerGraphic = GetComponentInChildren<Animator>().gameObject;
    }

    private void Update()
    {
        switch (state)
        {
            case CustomerState.Idle:
                movement.HandleTableArrival();
                break;

            case CustomerState.Leaving:
                behavior.HandleLeaving(this);
                break;
        }
    }

    public bool CanInteract(Interactor interactor)
    {
        if (interactor == null)
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        var playerItem = interactor.playerItem;

        if (!movement.IsArrived) return false;

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

    public void Interact(Interactor interactor)
    {
        var playerItem = interactor.playerItem;
        if(playerItem == null) 
            return;

        switch (state)
        {
            case CustomerState.Idle:
                Order();
                HandleArrival();
                break;

            case CustomerState.Ordered:
                orderSystem.ServeOrder(playerItem);
                break;
        }
    }

    void HandleArrival()
    {
        patience.StartPatienceTimer();
        OnDiscovered();
    }
        
    void HandlePatienceExpired()
    {
        state = CustomerState.Leaving;
        behavior.OnPatienceExpired(this);

        OnStateChanged?.Invoke(state);
        OnFinished?.Invoke(this);
    }

    void HandleOrder(bool b, int totalPrice)
    {
        if (b)
        {
            state = CustomerState.Leaving;
            OnStateChanged?.Invoke(state);

            behavior.OnCorrectServe(this, totalPrice);

            Debug.Log("All order served correctly");
        }
        else
        {
            state = CustomerState.Leaving;
            OnStateChanged?.Invoke(state);

            behavior.OnWrongServe(this);

            Debug.Log("Served at least one order wrong");
        }

        OnFinished?.Invoke(this);
    }

    public void Order()
    {
        orderSystem.GenerateOrder();
        state = CustomerState.Ordered;
        OnStateChanged?.Invoke(state);

        OnOrderTaken?.Invoke(this);
        Claimer = null;
        OnDiscovered();
    }

    public void WorkerOrderServe(Item item)
    {
        orderSystem.ServeOrderWorker(item);
    }

    public bool TrySetClaimer(IWorkerTask claimer)
    {
        if (Claimer != null && Claimer != claimer)
            return false;

        Claimer = claimer;
        return true;
    }
    
    public void ClearClaimer(IWorkerTask claimer)
    {
        if (Claimer == claimer)
            Claimer = null;
    }

    public void OnDiscovered() => WorkerEvents.NotifyTaskDiscovered(this);

    public Transform GetPosition() => transform;

    public CustomerState GetCurrentState() => state;
}
