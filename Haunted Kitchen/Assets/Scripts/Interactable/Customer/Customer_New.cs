using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Customer_New : MonoBehaviour, Iinteractable, IWorkerInteractable
{
    public CustomerMovement movement;
    public CustomerPatience patience;
    public CustomerOrder orderSystem;
    public CustomerUI ui;
    public CustomerBehavior behavior;

    public GameObject customerGraphic;
    [SerializeField] private CustomerState state = CustomerState.Idle;

    [SerializeField] private bool _isTargeted = false;
    public bool IsTargeted { get => _isTargeted; set => _isTargeted = value; }

    public event Action<CustomerState> OnStateChanged;
    public event Action<IWorkerInteractable> OnFinished;

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
    }

    public void Order()
    {
        orderSystem.GenerateOrder();
        state = CustomerState.Ordered;
        OnStateChanged?.Invoke(state);

        OnFinished?.Invoke(this);
    }

    public void WorkerOrderServe(List<ItemData> items)
    {
        bool isCorrect = orderSystem.ValidateWorkerOrder(items);

        state = CustomerState.Leaving;
        OnStateChanged?.Invoke(state);

        if (isCorrect)
        {
            behavior.OnCorrectServe(this, items.Sum(i => i.price));
            Debug.Log("Worker served correct order");
        }
        else
        {
            behavior.OnWrongServe(this);
            Debug.Log("Worker served wrong order");
        }

        OnFinished?.Invoke(this);
    }

    public void OnDiscovered() => WorkerEvents.NotifyTaskDiscovered(this);

    public Transform GetPosition() => transform;

    public CustomerState GetCurrentState() => state;
}
