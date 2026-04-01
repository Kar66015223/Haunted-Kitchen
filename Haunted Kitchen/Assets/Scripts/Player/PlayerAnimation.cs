using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private PlayerInteractableDetector detector;
    [SerializeField] private PlayerInputHandler inputHandler;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        detector = GetComponent<PlayerInteractableDetector>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    void OnEnable()
    {
        inputHandler.OnHoldProgressChanged += OnHoldProgressChanged;
    }

    void OnDisable()
    {
        inputHandler.OnHoldProgressChanged -= OnHoldProgressChanged;
    }

    private void OnHoldProgressChanged(float progress)
    {
        bool isCleaning = progress > 0f
                        && detector.GetCurrentInteractable() is ICleanable;

        SetClean(isCleaning);
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

    public void SetClean(bool value)
    {
        anim.SetBool(PlayerConstants.ANIM_CLEAN, value);
    }
}
