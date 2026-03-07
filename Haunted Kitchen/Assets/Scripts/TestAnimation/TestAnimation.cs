using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestAnimation : MonoBehaviour
{
    [Header("Cherry")]
    [SerializeField] private Character cherry;

    [Header("Tui")]
    [SerializeField] private Character tui;

    [Header("Customer")]
    [SerializeField] private Character customer;

    private void Start()
    {
        cherry = FindAnyObjectByType<Cherry>();
        tui = FindAnyObjectByType<Tui>();
        customer = FindAnyObjectByType<CustomerAnimTest>();
    }

    #region Cherry
    public void SetCherryIdle()
    {
        cherry.SetCherryState(CherryState.Idle);
    }

    public void SetCherryIdleOneHand()
    {
        cherry.SetCherryState(CherryState.IdleOneHand);
    }

    public void SetCherryIdleTwoHand()
    {
        cherry.SetCherryState(CherryState.IdleTwoHand);
    }

    public void SetCherryWalk()
    {
        cherry.SetCherryState(CherryState.Walk);
    }

    public void SetCherryWalkOneHand()
    {
        cherry.SetCherryState(CherryState.WalkOneHand);
    }

    public void SetCherryWalkTwoHand()
    {
        cherry.SetCherryState(CherryState.WalkTwoHand);
    }

    public void SetCherrySlip()
    {
        cherry.SetCherryState(CherryState.Slip);
    }

    public void SetCherryStruggle()
    {
        cherry.SetCherryState(CherryState.Struggle);
    }

    public void SetCherryPossessed()
    {
        cherry.SetCherryState(CherryState.Possessed);
    }
    #endregion

    #region Tui
    public void SetTuiIdle()
    {
        tui.SetTuiState(TuiState.Idle);
    }

    public void SetTuiWalk()
    {
        tui.SetTuiState(TuiState.Walk);
    }

    public void SetTuiPourOil()
    {
        tui.SetTuiState(TuiState.PourOil);
    }

    public void SetTuiAttack()
    {
        tui.SetTuiState(TuiState.Attack);
    }
    #endregion

    #region Customer
    public void SetCustomerWalk()
    {
        customer.SetCustomerAnimTestState(CustomerAnimTestState.Walk);
    }

    public void SetCustomerSit()
    {
        customer.SetCustomerAnimTestState(CustomerAnimTestState.Sit);
    }

    public void SetCustomerAttack()
    {
        customer.SetCustomerAnimTestState(CustomerAnimTestState.Attack);
    }
    #endregion
}
