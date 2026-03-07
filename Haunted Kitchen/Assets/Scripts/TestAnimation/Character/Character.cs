using System;
using UnityEngine;

public enum CherryState
{
    Idle,
    IdleOneHand,
    IdleTwoHand,
    Walk,
    WalkOneHand,
    WalkTwoHand,
    Slip,
    Struggle,
    Possessed
}

public enum TuiState
{
    Idle,
    Walk,
    PourOil,
    Attack
}

public enum CustomerAnimTestState
{
    Walk,
    Sit,
    Attack
}

public abstract class Character : MonoBehaviour
{
    protected Animator anim;
    public Animator Anim { get { return anim; } }

    protected CherryState cherryState;
    public CherryState CherryState { get { return cherryState; } }

    protected TuiState tuiState;
    public TuiState TuiState { get { return tuiState; } }

    protected CustomerAnimTestState customerState;
    public CustomerAnimTestState CustomerState { get { return customerState; } }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetCherryState(CherryState s)
    {
        cherryState = s;
    }

    public void SetTuiState(TuiState s)
    {
        tuiState = s;
    }

    public void SetCustomerAnimTestState(CustomerAnimTestState c)
    {
        customerState = c;
    }
}
