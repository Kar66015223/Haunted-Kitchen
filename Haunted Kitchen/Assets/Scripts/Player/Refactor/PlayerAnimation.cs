using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetState(int value)
    {
        anim.SetInteger(PlayerConstants.ANIM_STATE, value);
    }

    public void SetRun(bool isRunning, float multiplier)
    {
        anim.speed = isRunning ? multiplier : 1f;
    }

    public void SetHoldOneHand(bool value)
    {
        anim.SetBool(PlayerConstants.ANIM_IDLE_ONE_HAND, value);
    }

    public void SetHoldTwoHand(bool value)
    {
        anim.SetBool(PlayerConstants.ANIM_IDLE_TWO_HAND, value);
    }

    public void SetDie()
    {
        anim.SetTrigger(PlayerConstants.ANIM_DIE);
    }

    public void SetSlip()
    {
        anim.SetTrigger(PlayerConstants.ANIM_SLIP);
    }

    public void SetPossessed(bool value)
    {
        anim.SetBool(PlayerConstants.ANIM_ISPOSSESSED, value);
    }

    public void SetStruggle()
    {
        anim.SetTrigger(PlayerConstants.ANIM_STRUGGLE);
    }
}
