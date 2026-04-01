using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    public Button firstItemButton;
    [SerializeField] private Button closeButton;

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

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    void OnDestroy()
    {
        if (possession != null)
            possession.OnPossessionStarted -= OnPlayerPossessed;

        if (closeButton != null)
            closeButton.onClick.RemoveAllListeners();
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

        // if (input == null)
        // {
        //     input = GameObject.FindWithTag(PlayerConstants.PLAYER_TAG).GetComponent<PlayerInput>();
        // }
        // input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_UI);

        GameEvents.OnUIShow?.Invoke(true);

        if (IsUsingGamepad() && EventSystem.current != null && firstItemButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstItemButton.gameObject);
        }
    }

    public void Close()
    {
        EventSystem.current.SetSelectedGameObject(null);

        root.SetActive(false);

        GameEvents.OnUIShow?.Invoke(false);

        // if(input != null)
        // input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_PLAYER);
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
