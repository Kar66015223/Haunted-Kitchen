using UnityEngine;
using System;

public class Customer_New : MonoBehaviour, Iinteractable
{
    public CustomerMovement movement;
    public CustomerPatience patience;
    public CustomerOrder orderSystem;
    public CustomerUI ui;
    public CustomerBehavior behavior;
    [SerializeField] private CustomerState state = CustomerState.Idle;

    public event Action<CustomerState> OnStateChanged;

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
        if(interactor == null) 
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
                orderSystem.GenerateOrder();
                state = CustomerState.Ordered;
                OnStateChanged?.Invoke(state);

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
}
