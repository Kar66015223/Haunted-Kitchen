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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);

        if (move.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

        float currentSpeed = isRunning ? runSpeed : moveSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }
}
