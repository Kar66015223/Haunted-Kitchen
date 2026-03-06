using UnityEngine;

public class GhostPossessState : IGhostState
{
    private GhostController controller;
    private GhostPosessController possessController;
    
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
        possessController = controller.GetComponent<GhostPosessController>();
    }

    public void Enter()
    {
        Debug.Log("Ghost enters Possess state");

        GameManager.instance.ShowEventText("The ghost stares at you with malicious intent...", Color.red);

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
                // State complete, can transition to another state or disappear
                break;
        }
    }

    private void UpdateStaring()
    {
        // Look at player
        Transform playerPos = possessController.GetPlayer();
        controller.transform.LookAt(playerPos);
        
        stareTimer += Time.deltaTime;

        if (stareTimer >= stareTime && !hasDashed)
        {
            // Start dash phase
            InitiateDash();
            currentPhase = PossessPhase.Dashing;
            hasDashed = true;
        }
    }

    private void InitiateDash()
    {
        // Calculate dash direction toward player
        dashDirection = (controller.player.position - controller.transform.position).normalized;
        
        // Start dash timer
        dashTimer = 0f;
        
        Debug.Log($"Ghost dashing toward player: {dashDirection}");
    }

    private void UpdateDashing()
    {
        dashTimer += Time.deltaTime;
        float dashProgress = dashTimer / dashDuration;

        // Manually move ghost during dash (NavMeshAgent might interfere)
        Vector3 newPosition = controller.transform.position + (dashDirection * dashSpeed * Time.deltaTime);
        controller.transform.position = newPosition;

        // Check if dash is complete
        if (dashProgress >= 1f)
        {
            currentPhase = PossessPhase.Complete;
            OnDashComplete();
        }
        
        // Check if ghost has reached/passed player
        float distanceToPlayer = Vector3.Distance(controller.transform.position, controller.player.position);
        if (distanceToPlayer < 1f)
        {
            OnReachedPlayer();
        }
    }

    private void OnReachedPlayer()
    {
        Debug.Log("Ghost reached player!");
        
        //Start possessing
        
        Exit(); //Move this to possess method
    }

    private void OnDashComplete()
    {
        Debug.Log("Ghost dash complete");
        // Reset to idle or return to previous state

        Exit();
    }

    public void Exit()
    {
        Debug.Log("Ghost exits Possess state");
        controller.Disappear();
    }
}
