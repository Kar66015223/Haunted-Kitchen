using UnityEngine;

public class PlayerClean : MonoBehaviour
{
    [SerializeField] private GameObject mopPrefab;
    
    private PlayerInputHandler inputHandler;
    private PlayerAnimation anim;
    private PlayerInteractableDetector detector;
    private PlayerSound sound;

    void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        anim = GetComponent<PlayerAnimation>();
        detector = GetComponent<PlayerInteractableDetector>();
        sound = GetComponent<PlayerSound>();
    }

    void OnEnable()
    {
        inputHandler.OnHoldProgressChanged += OnHoldProgressChanged;
    }

    void OnDisable()
    {
        inputHandler.OnHoldProgressChanged -= OnHoldProgressChanged;
    }

    private void OnHoldProgressChanged(float progress)
    {
        bool isCleaning = progress > 0f
                        && detector.GetCurrentInteractable() is ICleanable;

        anim.SetClean(isCleaning);
        ToggleMop(isCleaning);

        if (isCleaning)
        {
            sound.PlayMopSound();
        }
        else
        {
            sound.StopAction();
        }
    }
    
    private void ToggleMop(bool value)
    {
        if (mopPrefab == null)
            {
                Debug.LogError("No mop object assigned");
                return;
            }

        mopPrefab.SetActive(value);
    }
}
