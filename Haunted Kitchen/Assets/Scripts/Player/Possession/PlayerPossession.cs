using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Xml.Serialization;

public class PlayerPossession : MonoBehaviour
{
    [Header("Possession")]
    [SerializeField] private float possessionDuration = 20f;
    [SerializeField] private float struggleThreshold = 100f;

    [Header("Struggle")]
    [SerializeField] private float struggleValue = 10f;
    [SerializeField] private float decayRate = 2f;

    // State
    private bool isPossessed = false;
    public bool IsPossessed => isPossessed;

    private float struggleProgress = 0f;
    private float possessionTimer = 0f;
    private GameObject possessingGhost;

    // references
    private PlayerController controller;
    private PlayerInput input;
    private Animator anim;

    // Events
    public event Action<float> OnStruggleProgressChanged;
    public event Action OnPossessionStarted;
    public event Action OnPossessionEnded;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        input = GetComponent<PlayerInput>();
        anim = controller.anim;
    }

    void Update()
    {
        if (!isPossessed) return;

        UpdatePossession();
        UpdateStruggle();
    }

    public void StartPossession(GameObject ghost)
    {
        if (isPossessed) return;

        isPossessed = true;
        possessingGhost = ghost;
        possessionTimer = 0f;
        struggleProgress = 0f;

        Debug.Log("Player Possessed");

        DisablePlayerInput();
        PlayPossessionAnimation();
        DropHeldItem();

        if (PossessionUIManager.instance != null)
        {
            PossessionUIManager.instance.ShowPossessionUI();
        }

        OnPossessionStarted?.Invoke();
    }

    private void UpdatePossession()
    {
        possessionTimer += Time.deltaTime;

        if (possessionTimer >= possessionDuration)
        {
            EndPossession(false);
        }
    }

    private void UpdateStruggle()
    {
        if (struggleProgress > 0f)
        {
            struggleProgress -= decayRate * Time.deltaTime;
            struggleProgress = Mathf.Max(0f, struggleProgress);
        }

        OnStruggleProgressChanged?.Invoke(GetStruggleProgress());

        if (struggleProgress >= struggleThreshold)
        {
            EndPossession(true);
        }
    }

    public void OnStruggleInput(InputAction.CallbackContext context)
    {
        if (!isPossessed) return;

        if (context.performed)
        {
            AddStruggle(struggleValue);

            if(anim != null)
            {
                anim.SetTrigger(PlayerConstants.ANIM_STRUGGLE);
            }

            Debug.Log($"Struggle progress: {struggleProgress} / {struggleThreshold}");
        }
    }

    private void AddStruggle(float amount)
    {
        struggleProgress += amount;
        OnStruggleProgressChanged?.Invoke(GetStruggleProgress());
    }

    // Param = true if player broke free, false if not.
    private void EndPossession(bool successfullyFreed)
    {
        if (!isPossessed) return;

        isPossessed = false;

        EnablePlayerInput();
        StopPossessionAnimation();

        if (PossessionUIManager.instance != null)
        {
            PossessionUIManager.instance.HidePossessionUI();
        }

        if (successfullyFreed)
        {
            Debug.Log("Player broke free");
        }
        else
        {
            Debug.Log("Player failed to break free");

            // Punish player
        }

        possessingGhost = null;
        OnPossessionEnded?.Invoke();
    }

    private void DisablePlayerInput()
    {
        if (input != null)
        {
            input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_POSSESSION);
        }

        // controller.Slip(0f); // Use slip with 0 duration to stop movement
    }

    private void EnablePlayerInput()
    {
        if (input != null)
        {
            input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_PLAYER);
        }
    }

    private void DropHeldItem()
    {
        PlayerItem playerItem = GetComponent<PlayerItem>();

        if (playerItem == null)
        {
            Debug.LogError("No playerItem found in player");
            return;
        }

        playerItem.DropItemNoRaycast();
    }

    private void PlayPossessionAnimation()
    {
        if (anim == null) return;

        anim.SetBool(PlayerConstants.ANIM_ISPOSSESSED, true);
    }

    private void StopPossessionAnimation()
    {
        if (anim == null) return;

        anim.SetBool(PlayerConstants.ANIM_ISPOSSESSED, false);
    }

    public bool CanPerformAction()
    {
        return !isPossessed;
    }
    
    public float GetStruggleProgress()
    {
        return Mathf.Clamp01(struggleProgress / struggleThreshold);
    }
}
