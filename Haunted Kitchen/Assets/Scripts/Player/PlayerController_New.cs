using UnityEngine;

[RequireComponent(typeof(IPlayerMovementController))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerStamina))]
[RequireComponent(typeof(PlayerInteract_New))]
[RequireComponent(typeof(PlayerItem))]
[RequireComponent(typeof(PlayerAnimation))]
public class PlayerController_New : MonoBehaviour
{
    [SerializeField] private float moveSpeed = PlayerConstants.MOVE_SPEED;
    [SerializeField] private float runSpeed = PlayerConstants.RUN_SPEED;
    [SerializeField] private float runAnimMultiplier = PlayerConstants.RUN_ANIM_MULTIPLIER;

    // Slipping
    [SerializeField] private bool isSlipping;
    public bool IsSlipping => isSlipping;

    private float slipTimer;
    [SerializeField] private GameObject slipVFX;

    // Speed Buff
    private float speedBuffTimer;
    private bool hasSpeedBuff;
    [SerializeField] private GameObject speedBuffVFX;

    // Cross
    private PlayerCross playerCross;

    // Components
    private PlayerInputHandler inputHandler;
    private IPlayerMovementController movement;
    private PlayerStamina stamina;
    private PlayerAnimation anim;
    private PlayerInteract_New playerInteract;
    private PlayerItem playerItem;
    private PauseManager pauseManager;

    private Vector3 currentFacingDirection = Vector3.forward;

    void Awake()
    {
        movement = GetComponent<IPlayerMovementController>();
        inputHandler = GetComponent<PlayerInputHandler>();
        stamina = GetComponent<PlayerStamina>();
        playerInteract = GetComponent<PlayerInteract_New>();
        playerItem = GetComponent<PlayerItem>();
        playerCross = GetComponent<PlayerCross>();

        anim = GetComponent<PlayerAnimation>();

        if (movement == null)
            Debug.LogError("IPlayerMovementController not found!");

        if (inputHandler == null)
            Debug.LogError("PlayerInputHandler not found!");

        if (stamina == null)
            Debug.LogError("PlayerStamina not found!");

        if (playerInteract == null)
            Debug.LogError("PlayerInteract_New not found!");

        if (playerItem == null)
            Debug.LogError("PlayerItem not found!");

        if (playerCross == null)
            Debug.LogError("PlayerCross not found!");

        if (anim == null)
            Debug.LogError("PlayerAnimation not found!");
    }

    void OnEnable()
    {
        inputHandler.OnInteractInput += HandleInteract;
        inputHandler.OnHoldInteractInput += HandleHoldInteract;
        inputHandler.OnDropInput += HandleDrop;

        inputHandler.OnPauseInput += HandlePause;

        inputHandler.OnHoldCrossInput += HandleHoldCross;
        inputHandler.OnHoldCrossInputCanceled += HandleHoldCrossCancel;

        GameEvents.OnSpeedBuff += ApplySpeedBuff;
    }

    void OnDisable()
    {
        inputHandler.OnInteractInput -= HandleInteract;
        inputHandler.OnHoldInteractInput -= HandleHoldInteract;
        inputHandler.OnDropInput -= HandleDrop;

        inputHandler.OnPauseInput -= HandlePause;

        inputHandler.OnHoldCrossInput -= HandleHoldCross;
        inputHandler.OnHoldCrossInputCanceled -= HandleHoldCrossCancel;

        GameEvents.OnSpeedBuff -= ApplySpeedBuff;
    }

    void Start()
    {
        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    void Update()
    {
        if (isSlipping)
        {
            if (Time.time >= slipTimer)
            {
                isSlipping = false;
                slipVFX.SetActive(false);
                SetCanMove(true);
            }

            return;
        }

        if (hasSpeedBuff && Time.time >= speedBuffTimer)
        {
            hasSpeedBuff = false;
            speedBuffVFX.SetActive(false);
        }

        if(playerCross != null && playerCross.IsHoldingCross)
        {
            SetCanMove(false);
            return;
        }
        
        HandleMoveInput();
    }

    private void HandleMoveInput()
    {
        // Move
        Vector2 moveInput = inputHandler.MoveInput;
        bool isRunning = inputHandler.IsRunning;

        if (moveInput.sqrMagnitude < 0.001f)
        {
            StopMoving();
            return;
        }

        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Rotate
        currentFacingDirection = moveDirection;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            PlayerConstants.ROTATION_SPEED * Time.deltaTime
        );

        float buffMultiplier = hasSpeedBuff ? 2f : 1f;
        float speed = (isRunning ? runSpeed : moveSpeed) * buffMultiplier;

        // Stamina
        bool canRun = isRunning && stamina != null && stamina.CanRun();
        if (canRun && moveInput.sqrMagnitude > 0.001f)
        {
            stamina.Drain(stamina.drainRate * Time.deltaTime);
        }

        if(stamina != null && !stamina.CanRun())
        {
            speed = moveSpeed;
        }

        movement.Move(moveDirection, speed);

        // Anim
        if (moveInput.sqrMagnitude > 0.001f)
        {
            anim.SetState(1);
            anim.SetRun(isRunning, runAnimMultiplier);
        }
    }

    public void Slip(float duration)
    {
        if (isSlipping || hasSpeedBuff) return;

        isSlipping = true;
        slipTimer = Time.time + duration;
        SetCanMove(false);

        if (playerItem != null)
        {
            playerItem.DropItemNoRaycast();
        }

        // isRunning = false;

        slipVFX?.SetActive(true);

        anim.SetRun(false, 1f);
        anim.SetSlip();

        Debug.Log("Slip called with duration: " + duration);
    }

    private void ApplySpeedBuff(float duration)
    {
        hasSpeedBuff = true;
        speedBuffTimer = Time.time + duration;

        speedBuffVFX.SetActive(true);
    }

    public void SetCanMove(bool canMove)
    {
        if (movement != null)
        {
            movement.SetCanMove(canMove);
            if (!canMove)
            {
                StopMoving();
            }
        }
    }

    public void StopMoving()
    {
        movement.Stop();
        anim?.SetState(0);
    }

    public Vector3 GetFacingDirection()
    {
        return currentFacingDirection;
    }

    private void HandleInteract()
    {
        playerInteract.TryInteract();
    }

    private void HandleHoldInteract()
    {
        playerInteract.TryHoldInteract();
    }

    private void HandleDrop()
    {
        playerItem.DropItem();
    }

    private void HandlePause()
    {
        pauseManager.Pause();
    }

    private void HandleHoldCross()
    {
        if (playerItem.currentHeldItemObj != null)
            return;
            
        playerCross.HoldCross();
    }

    private void HandleHoldCrossCancel()
    {
        if (playerItem.currentHeldItemObj != null)
            return;
            
        playerCross.PutCrossAway();
    }
}
