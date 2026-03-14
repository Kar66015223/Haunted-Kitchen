using UnityEngine;
using UnityEngine.InputSystem;
using System;
using NUnit.Framework;

public class PlayerInputHandler : MonoBehaviour
{
    // Move
    private Vector2 moveInput = Vector2.zero;
    public Vector2 MoveInput => moveInput;

    private bool isRunning = false;
    public bool IsRunning => isRunning;

    // Interaction
    private float interactStartTime;
    private bool isHoldingInteract;
    private bool holdTriggered;
    [SerializeField] private float holdThreshold = PlayerConstants.HOLD_THRESHOLD;

    public event Action OnInteractInput;
    public event Action OnHoldInteractInput;
    public event Action<float> OnHoldProgressChanged;

    public event Action OnDropInput;

    // UI
    public event Action OnPauseInput;

    // Cross
    private bool isHoldingCross = false;
    public bool IsHoldingCross => isHoldingCross;

    public event Action OnHoldCrossInput;
    public event Action OnHoldCrossInputCanceled;

    void Update()
    {
        HandleHoldInteraction();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            moveInput = Vector2.zero;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            interactStartTime = Time.time;
            isHoldingInteract = true;
            holdTriggered = false;
        }

        if (context.canceled)
        {
            if (!holdTriggered)
            {
                OnInteractInput?.Invoke();
            }

            isHoldingInteract = false;
            OnHoldProgressChanged?.Invoke(0f);
        }
    }
    
    private void HandleHoldInteraction()
    {
        if (!isHoldingInteract || holdTriggered)
            return;

        float heldTime = Time.time - interactStartTime;

        // Progress (0 to 1)
        float progress = Mathf.Clamp01(heldTime / holdThreshold);
        OnHoldProgressChanged?.Invoke(progress);

        if(heldTime >= holdThreshold)
        {
            holdTriggered = true;
            OnHoldProgressChanged?.Invoke(0f);
            OnHoldInteractInput?.Invoke();
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnDropInput?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPauseInput?.Invoke();
        }
    }

    public void OnHoldCross(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnHoldCrossInput?.Invoke();
            isHoldingCross = true;
        }

        if (context.canceled)
        {
            OnHoldCrossInputCanceled?.Invoke();
            isHoldingCross = false;
        }
    }
}
