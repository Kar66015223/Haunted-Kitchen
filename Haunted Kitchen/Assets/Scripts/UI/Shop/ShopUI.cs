using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    public Button firstItemButton;

    [SerializeField] private PlayerInput input;
    [SerializeField] private PlayerPossession possession;
 
    private void Awake()
    {
        root.SetActive(false);
    }

    void Start()
    {
        input = GameObject.FindWithTag(PlayerConstants.PLAYER_TAG).GetComponent<PlayerInput>();
        possession = GameObject.FindWithTag(PlayerConstants.PLAYER_TAG).GetComponent<PlayerPossession>();

        if (possession != null)
            possession.OnPossessionStarted += OnPlayerPossessed;
    }

    void OnDestroy()
    {
        if (possession != null)
            possession.OnPossessionStarted -= OnPlayerPossessed;
    }

    void OnPlayerPossessed()
    {
        if(root.activeSelf)
        {
            Close();
            input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_POSSESSION);
        }
    }

    public void Open()
    {
        root.SetActive(true);

        if (input != null)
            input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_UI);

        if (IsUsingGamepad() && EventSystem.current != null && firstItemButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstItemButton.gameObject);
        }
    }

    public void Close()
    {
        EventSystem.current.SetSelectedGameObject(null);

        root?.SetActive(false);
        input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_PLAYER);
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Close();
    }

    bool IsUsingGamepad()
    {
        if (input == null) return false;

        return input.currentControlScheme == "Gamepad";
    }
}
