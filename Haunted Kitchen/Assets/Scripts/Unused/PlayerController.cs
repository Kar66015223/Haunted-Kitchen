using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 9f;

    private CharacterController controller;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

    public float gravity = -20f;
    public float groundedForce = -2f;

    private float verticalVelocity;

    private bool isRunning;
    public bool IsRunning => isRunning && stamina != null && stamina.CanRun();

    private PlayerStamina stamina;
    private PlayerInteract playerInteract;
    private PlayerItem playerItem;
    private PlayerPossession possession;

    [Header("Animation")]
    public Animator anim;
    public float runAnimMultiplier = 1.5f;

    [Header("Slipping")]
    [SerializeField] private bool isSlipping;
    public bool IsSlipping => isSlipping;

    [SerializeField] private float slipTimer;
    [SerializeField] private GameObject slipVFX;

    [Header("Speed Buff")]
    private float speedBuffTimer;
    private bool hasSpeedBuff;
    [SerializeField] private GameObject speedBuffVFX;

    [Header("Hold Interact")]
    private float interactStartTime;
    private bool isHoldingInteract;
    private bool holdTriggered;
    [SerializeField] private float holdThreshold = 0.4f;

    public event Action<float> OnHoldProgressChanged;

    private void OnEnable()
    {
        GameEvents.OnSpeedBuff += ApplySpeedBuff;
    }
    private void OnDisable()
    {
        GameEvents.OnSpeedBuff -= ApplySpeedBuff;

        anim.speed = 1f;
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        stamina = GetComponent<PlayerStamina>();
        playerInteract = GetComponent<PlayerInteract>();
        playerItem = GetComponent<PlayerItem>();
        possession = GetComponent<PlayerPossession>();
    }

    private void Start()
    {
        anim.SetInteger(PlayerConstants.ANIM_STATE, 0);
    }

    void Update()
    {

        if (isSlipping)
        {
            if (Time.time >= slipTimer)
            {
                isSlipping = false;
                slipVFX.SetActive(false);
            }

            return;
        }

        if (isHoldingInteract && 
            !holdTriggered)
        {
            if (playerInteract != null && 
                playerInteract.CanHoldCurrentInteractable())
            {
                float heldTime = Time.time - interactStartTime;
                float progress = Mathf.Clamp01(heldTime / holdThreshold);

                OnHoldProgressChanged?.Invoke(progress);

                if (heldTime >= holdThreshold)
                {
                    holdTriggered = true;
                    OnHoldProgressChanged?.Invoke(0f);
                    playerInteract?.TryHoldInteract();
                } 
            }
            else
            {
                OnHoldProgressChanged?.Invoke(0f);
            }
        }
        if (!isHoldingInteract)
        {
            OnHoldProgressChanged?.Invoke(0f);
        }

        if (hasSpeedBuff && Time.time >= speedBuffTimer)
        {
            hasSpeedBuff = false;
            speedBuffVFX.SetActive(false);
        }

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation,
            targetRotation,
            PlayerConstants.ROTATION_SPEED * Time.deltaTime);
        }

        bool canRun = isRunning && stamina != null && stamina.CanRun();
        float buffMultiplier = hasSpeedBuff ? 2f : 1f;
        float currentSpeed = (canRun ? runSpeed : moveSpeed) * buffMultiplier;

        //Moving
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedForce;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = move * currentSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

        if (canRun && move.sqrMagnitude > 0.001f) // if player is moving and running, drain stamina
        {
            stamina.Drain(stamina.drainRate * Time.deltaTime);
        }

        #region Animation
        bool isMoving = moveInput.sqrMagnitude > 0.001f;
        bool isRunningNow = IsRunning;

        anim.SetInteger(PlayerConstants.ANIM_STATE, isMoving ? 1 : 0);

        if (isMoving)
        {
            anim.speed = isRunningNow ? runAnimMultiplier : 1f;
        }
        else
        {
            anim.speed = 1f;
        } 
        #endregion
    }

    public void Slip(float duration)
    {
        if (isSlipping) return;

        isSlipping = true;
        slipTimer = Time.time + duration;

        if (playerItem != null)
        {
            playerItem.DropItemNoRaycast();
        }

        isRunning = false;

        slipVFX?.SetActive(true);

        anim.speed = 1f;
        anim.SetTrigger(PlayerConstants.ANIM_SLIP);

        Debug.Log("Slip called with duration: " + duration);
    }

    private void ApplySpeedBuff(float duration)
    {
        hasSpeedBuff = true;
        speedBuffTimer = Time.time + duration;

        speedBuffVFX.SetActive(true);
    }
    
    #region InputAction
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (isSlipping) return;

        if (context.started)
        {
            interactStartTime = Time.time;
            isHoldingInteract = true;
            holdTriggered = false;
        }

        if(context.canceled)
        {
            if (!holdTriggered)
            {
                playerInteract?.TryInteract();
            }

            isHoldingInteract = false;
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed && playerItem != null)
        {
            playerItem.DropItem();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed || GameManager.instance == null) return;

        if (GameManager.instance.isPaused)
            GameManager.instance.UnPause();

        else
            GameManager.instance.Pause();
    } 
    
    public void OnStruggle(InputAction.CallbackContext context)
    {
        possession.OnStruggleInput(context);
    }
    #endregion
}
