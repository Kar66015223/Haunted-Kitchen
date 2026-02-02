using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

    private void Awake()
    {
        root.SetActive(false);
    }

    public void Open()
    {
        root.SetActive(true);

        if (input != null)
            input.SwitchCurrentActionMap("UI");

        if (IsUsingGamepad() && EventSystem.current != null && firstItemButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstItemButton.gameObject);
        }
    }

    public void Close()
    {
        EventSystem.current.SetSelectedGameObject(null);

        root?.SetActive(false);
        input.SwitchCurrentActionMap("Player");
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
