using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerMovementController
{
    [SerializeField] private float gravity = PlayerConstants.GRAVITY;
    [SerializeField] private float groundedForce = PlayerConstants.GROUNDED_FORCE;
    
    private CharacterController controller;
    private float verticalVelocity = 0f;
    private bool canMove = true;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("CharacterController not found on " + gameObject.name);
        }
    }

    public void Move(Vector3 direction, float speed)
    {
        if (!canMove || controller == null)
            return;

        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedForce;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = direction * speed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);
    }

    public void SetCanMove(bool canMove)
    {
        this.canMove = canMove;
    }

    public void Stop()
    {
        // Reset vertical velocity
        verticalVelocity = 0f;

        // Apply minimal movement to keep character controller happy
        if (controller != null && controller.isGrounded)
        {
            controller.Move(Vector3.down * Time.deltaTime);
        }
    }

    public void ApplyVelocity(Vector3 velocity) // Directly set velocity(for knockback or jump)
    {
        if(controller != null)
        {
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
