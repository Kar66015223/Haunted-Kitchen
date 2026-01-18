using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeed = 9f;

    private CharacterController controller;
    private Vector2 moveInput;
    private bool isRunning;

    private PlayerStamina stamina;
    private PlayerInteract playerInteract;
    private PlayerItem playerItem;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        stamina = GetComponent<PlayerStamina>();
        playerInteract = GetComponent<PlayerInteract>();
        playerItem = GetComponent<PlayerItem>();
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        bool canRun = isRunning && stamina != null && stamina.CanRun(); // if player is running, CanRun = true
        float currentSpeed = canRun ? runSpeed : moveSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        if (canRun && move.sqrMagnitude > 0.001f) // if player is moving and running, drain stamina
        {
            stamina.Drain(stamina.drainRate * Time.deltaTime);
        }
    }

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
        if (context.performed && playerInteract != null)
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
}
