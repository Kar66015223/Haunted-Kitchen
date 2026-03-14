using System;
using UnityEngine;

public class GhostPossessState : IGhostState
{
    private GhostController controller;
    private PlayerPossession playerPossession;
    private EventTextUI eventText;
    
    // Timing
    private float stareTimer = 0f;
    private float stareTime = 2.5f;
    private bool hasDashed = false;
    
    // Movement
    private float dashSpeed = 25f;
    private float dashDuration = 0.5f;
    private float dashTimer = 0f;
    private Vector3 dashDirection = Vector3.zero;
    
    // State tracking
    private enum PossessPhase { Staring, Dashing, Complete }
    private PossessPhase currentPhase = PossessPhase.Staring;

    public GhostPossessState(GhostController controller, float stareTime = 2.5f, float dashSpeed = 25f)
    {
        this.controller = controller;
        this.stareTime = stareTime;
        this.dashSpeed = dashSpeed;

        if (controller.player != null)
        {
            playerPossession = controller.player.GetComponent<PlayerPossession>();
        }
    }

    public void Enter()
    {
        Debug.Log("Ghost enters Possess state");

        GameEvents.OnShowEventText?.Invoke(
            GhostConstants.STATE_POSSESS_STARING_EVENT_TEXT, Color.red);

        stareTimer = 0f;
        dashTimer = 0f;
        hasDashed = false;
        currentPhase = PossessPhase.Staring;

        // Stop movement during stare phase
        controller.Movement.Stop();
    }

    public void Update()
    {
        if (controller.player == null) return;

        switch (currentPhase)
        {
            case PossessPhase.Staring:
                UpdateStaring();
                break;
            case PossessPhase.Dashing:
                UpdateDashing();
                break;
            case PossessPhase.Complete:
                break;
        }
    }

    private void UpdateStaring()
    {
        Vector3 directionToPlayer = (controller.player.position - controller.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        controller.transform.rotation = Quaternion.Slerp(
            controller.transform.rotation,
            targetRotation,
            Time.deltaTime * 5f
        );

        stareTimer += Time.deltaTime;

        if (stareTimer >= stareTime && !hasDashed)
        {
            InitiateDash();
            currentPhase = PossessPhase.Dashing;
            hasDashed = true;
        }
    }

    private void InitiateDash()
    {
        dashDirection = (controller.player.position - controller.transform.position).normalized;
        dashTimer = 0f;

        Collider col = controller.GetComponent<Collider>();
        col.enabled = false;

        GhostMovement movement = controller.Movement as GhostMovement;
        movement.DisableAgent();

        Debug.Log($"Ghost dashing toward player: {dashDirection}");
    }

    private void UpdateDashing()
    {
        dashTimer += Time.deltaTime;
        float dashProgress = dashTimer / dashDuration;

        dashDirection = (controller.player.position - controller.transform.position).normalized;

        Vector3 newPosition = controller.transform.position + (dashDirection * dashSpeed * Time.deltaTime);
        controller.Movement.Stop();
        controller.transform.position = newPosition;

        // if (dashProgress >= 1f)
        // {
        //     currentPhase = PossessPhase.Complete;
        //     OnDashComplete();
        // }
        
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
        if (distanceToPlayer < 1f)
        {
            OnReachedPlayer();
            currentPhase = PossessPhase.Complete;
        }
    }

    private void OnReachedPlayer()
    {
        Debug.Log("Ghost reached player!");

        if (playerPossession != null)
        {
            playerPossession.StartPossession(controller.gameObject);
        }
        else
        {
            Debug.LogWarning("PlayerPossessionController not found!");
        }
        
        Exit(); // Ghost will exit when possession ends or fails
    }

    // private void OnDashComplete()
    // {
    //     Debug.Log("Ghost dash complete");
    //     Exit();
    // }

    public void Exit()
    {
        Debug.Log("Ghost exits Possess state");
        controller.Disappear();
    }
}
