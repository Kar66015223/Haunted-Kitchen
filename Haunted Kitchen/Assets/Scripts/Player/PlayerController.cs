using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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
    public Vector2 MoveInput
    {
        get
        {
            return moveInput;
        }
    }

    public float gravity = -20f;
    public float groundedForce = -2f;

    private float verticalVelocity;

    private bool isRunning;
    public bool IsRunning => isRunning && stamina != null && stamina.CanRun();

    private PlayerStamina stamina;
    private PlayerInteract playerInteract;
    private PlayerItem playerItem;

    [Header("Animation")]
    public Animator anim;
    public float runAnimMultiplier = 1.5f;

    [Header("Slipping")]
    [SerializeField] private bool isSlipping;
    public bool IsSlipping { get { return isSlipping; } }

    [SerializeField] private float slipTimer;
    [SerializeField] private GameObject slipVFX;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        stamina = GetComponent<PlayerStamina>();
        playerInteract = GetComponent<PlayerInteract>();
        playerItem = GetComponent<PlayerItem>();
    }

    private void Start()
    {
        anim.SetInteger("State", 0);
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

        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        bool canRun = isRunning && stamina != null && stamina.CanRun();
        float currentSpeed = canRun ? runSpeed : moveSpeed;

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

        bool isMoving = moveInput.sqrMagnitude > 0.001f;
        bool isRunningNow = IsRunning;

        anim.SetInteger("State", isMoving ? 1 : 0);

        if (isMoving)
        {
            anim.speed = isRunningNow ? runAnimMultiplier : 1f;
        }
        else
        {
            anim.speed = 1f;
        }
    }

    public void Slip(float duration)
    {
        if (isSlipping) return;

        isSlipping = true;
        slipTimer = Time.time + duration;

        if (playerItem != null)
        {
            playerItem.DropItem();
        }

        isRunning = false;

        slipVFX?.SetActive(true);

        anim.speed = 1f;
        anim.SetTrigger("Slip");

        Debug.Log("Slip called with duration: " + duration);
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
        if (context.performed && playerInteract != null && !isSlipping)
        {
            playerInteract.TryInteract();
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
    #endregion

    private void OnDisable()
    {
        anim.speed = 1f;
    }
}
