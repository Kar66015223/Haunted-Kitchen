using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chair : MonoBehaviour, Iinteractable
{
    [SerializeField] private Transform customerStandPoint;
    public Transform CustomerStandPoint => customerStandPoint;

    [SerializeField] private CustomerBehavior currentCustomer;
    private System.Type currentCustomerType;
    public CustomerBehavior CurrentCustomer => currentCustomer;

    public bool isOccupied;

    private Animator visualAnim;
    
    [SerializeField] private List<SitVisual> sitVisuals = new();
    private Dictionary<System.Type, GameObject> visualLookup;

    void Awake()
    {
        visualLookup = new Dictionary<System.Type, GameObject>();

        foreach (var pair in sitVisuals)
        {
            System.Type behaviorType = pair.BehaviorType;
            if (behaviorType != null && !visualLookup.ContainsKey(behaviorType))
            {
                visualLookup.Add(behaviorType, pair.visual);
                pair.visual.SetActive(false);
            }
        }
    }

    public bool CanInteract(Interactor interactor)
    {
        Customer_New customer = currentCustomer?.GetComponent<Customer_New>();
        if (customer == null) return false;

        if(interactor == null) 
            return false;

        if (interactor.interactionType == InteractionType.Hold)
            return false;

        return customer.CanInteract(interactor);
    }

    public void Interact(Interactor interactor)
    {
        Customer_New customer = currentCustomer?.GetComponent<Customer_New>();
        if (customer == null) return;

        customer.Interact(interactor);
    }

    public void SetCurrentCustomer(Customer_New customer)
    {
        currentCustomer = customer.GetComponent<CustomerBehavior>();
        if (currentCustomer != null)
        {
            currentCustomerType = currentCustomer.GetType();
        }

        EnableVisual();
    }

    public void ClearCustomer()
    {
        DisableVisual();
        currentCustomer = null;
        currentCustomerType = null;
        isOccupied = false;
    }

    public void EnableVisual()
    {
        if (currentCustomer != null)
        {
            if (visualLookup.TryGetValue(currentCustomerType, out GameObject visual))
            {
                visual.SetActive(true);
                visualAnim = visual.GetComponentInChildren<Animator>();
                if (visualAnim != null)
                    visualAnim.SetBool(CustomerConstants.ANIM_SIT, true);
            }
        }
    }
    
    public void DisableVisual()
    {
        if (currentCustomer != null)
        {
            if(visualLookup.TryGetValue(currentCustomerType, out GameObject visual))
            {
                visual.SetActive(false);
                visualAnim = visual.GetComponentInChildren<Animator>();
                if (visualAnim != null)
                    visualAnim.SetBool(CustomerConstants.ANIM_SIT, false);
            }
        }
    }
}
