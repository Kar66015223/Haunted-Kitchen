using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PossessionUIManager : MonoBehaviour
{
    public static PossessionUIManager instance;

    [Header("Possession UI")]
    [SerializeField] private GameObject possessionUIPanel;
    [SerializeField] private Image struggleBar;
    [SerializeField] private TMP_Text instructionText;

    private PlayerPossession possession;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        possession = FindAnyObjectByType<PlayerPossession>();

        if (possession != null)
        {
            possession.OnStruggleProgressChanged += UpdateStruggleBar;
            possession.OnPossessionStarted += ShowPossessionUI;
            possession.OnPossessionEnded += HidePossessionUI;
        }

        possessionUIPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (possession != null)
        {
            possession.OnStruggleProgressChanged -= UpdateStruggleBar;
            possession.OnPossessionStarted -= ShowPossessionUI;
            possession.OnPossessionEnded -= HidePossessionUI;
        }
    }

    public void ShowPossessionUI()
    {
        possessionUIPanel.SetActive(true);

        if (instructionText != null)
        {
            instructionText.text = "SPAM SPACEBAR TO BREAK FREE!";
        }

        if (struggleBar != null)
        {
            struggleBar.fillAmount = 0f;
        }
    }

    public void HidePossessionUI()
    {
        possessionUIPanel.SetActive(false);
    }
    
    private void UpdateStruggleBar(float progress)
    {
        if(struggleBar != null)
        {
            struggleBar.fillAmount = progress;
        }
    }
}
