using UnityEngine;
using UnityEngine.UI;

public class AddMoneyButton : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(InvokeClicked);
    }

    void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    void InvokeClicked()
    {
        GameEvents.OnAddMoneyButtonClicked?.Invoke();
    }
}
