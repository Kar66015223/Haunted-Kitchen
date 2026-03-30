using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> allRecipes;
    [SerializeField] private int _currentIndex = 0;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button openButton;

    [SerializeField] private GameObject uiPanel;

    private PlayerInput input;
    private PlayerPossession playerPossess;

    public int CurrentIndex
    {
        get => _currentIndex;
        set => _currentIndex = Mathf.Clamp(value, 0, allRecipes.Count - 1);
    }

    void Start()
    {
        ShowRecipe(0);

        input = GameObject.FindWithTag(PlayerConstants.PLAYER_TAG)
            .GetComponent<PlayerInput>();
        playerPossess = GameObject.FindWithTag(PlayerConstants.PLAYER_TAG)
            .GetComponent<PlayerPossession>();

        nextButton.onClick.AddListener(NextRecipe);
        previousButton.onClick.AddListener(PreviousRecipe);

        if (playerPossess != null)
            playerPossess.OnPossessionStarted += OnPlayerPossessed;
    }

    void OnDestroy()
    {
        if (playerPossess != null)
            playerPossess.OnPossessionStarted -= OnPlayerPossessed;
    }

    void Update()
    {
        nextButton.gameObject.SetActive(CurrentIndex < allRecipes.Count - 1);
        previousButton.gameObject.SetActive(CurrentIndex > 0);

        if (input != null)
        {
            openButton.gameObject.SetActive(
                input.currentActionMap.name == PlayerConstants.INPUTACTION_PLAYER);
        }
    }

    public void Open()
    {   
        uiPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        uiPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void OnPlayerPossessed()
    {
        if (uiPanel.activeSelf)
        {
            Close();
            input.SwitchCurrentActionMap(PlayerConstants.INPUTACTION_POSSESSION);
        }
    }

    public void NextRecipe()
    {
        CurrentIndex++;
        ShowRecipe(CurrentIndex);
    }

    public void PreviousRecipe()
    {
        CurrentIndex--;
        ShowRecipe(CurrentIndex);
    }
    
    private void ShowRecipe(int index)
    {
        for(int i = 0; i < allRecipes.Count; i++)
        {
            allRecipes[i].SetActive(i == index);
        }
    }
}