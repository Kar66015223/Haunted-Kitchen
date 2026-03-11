using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController_New : MonoBehaviour
{
    [SerializeField] private float moveSpeed = PlayerConstants.MOVE_SPEED;
    [SerializeField] private float runSpeed = PlayerConstants.RUN_SPEED;
    [SerializeField] private float runAnimMultiplier = PlayerConstants.RUN_ANIM_MULTIPLIER;

    private PlayerInputHandler inputHandler;
    private IPlayerMovementController movement;
    private PlayerStamina stamina;
    private PlayerAnimation anim;
    private PlayerInteract_New playerInteract;
    private PlayerItem playerItem;

    private Vector3 currentFacingDirection = Vector3.forward;

    void Awake()
    {
        movement = GetComponent<IPlayerMovementController>();
        inputHandler = GetComponent<PlayerInputHandler>();
        stamina = GetComponent<PlayerStamina>();
        playerInteract = GetComponent<PlayerInteract_New>();
        playerItem = GetComponent<PlayerItem>();

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

        if (anim == null)
            Debug.LogError("PlayerAnimation not found!");
    }

    void OnEnable()
    {
        inputHandler.OnInteractInput += HandleInteract;
        inputHandler.OnHoldInteractInput += HandleHoldInteract;
        inputHandler.OnDropInput += HandleDrop;
        // inputHandler.OnPauseInput += HandlePause;
    }

    void OnDisable()
    {
        inputHandler.OnInteractInput -= HandleInteract;
        inputHandler.OnHoldInteractInput -= HandleHoldInteract;
        inputHandler.OnDropInput -= HandleDrop;
        // inputHandler.OnPauseInput -= HandlePause;
    }

    void Update()
    {
        HandleMoveInput();
    }

    private void HandleMoveInput()
    {
        // Move
        Vector2 moveInput = inputHandler.MoveInput;
        bool isRunning = inputHandler.IsRunning;

        if (moveInput.sqrMagnitude < 0.001f)
        {
            movement.Stop();
            anim?.SetState(0);
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

        float speed = isRunning ? runSpeed : moveSpeed;

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
        anim.SetState(1);
        anim.SetRun(isRunning, runAnimMultiplier);
    }

    public void SetCanMove(bool canMove)
    {
        if (movement != null)
        {
            movement.SetCanMove(canMove);
        }
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
}
