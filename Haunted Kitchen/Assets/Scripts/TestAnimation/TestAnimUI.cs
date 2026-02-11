using UnityEngine;
using UnityEngine.UI;

public class TestAnimUI : MonoBehaviour
{
    public GameObject characterChoose;
    public GameObject animChoose;

    public GameObject[] animPanels;

    private void Start()
    {
        animPanels = GameObject.FindGameObjectsWithTag("AnimPanel");

        ShowCharacterChoose();
    }

    public void ShowCharacterChoose()
    {
        characterChoose.SetActive(true);
        animChoose.SetActive(false);

        foreach (GameObject panel in animPanels)
        {
            panel.SetActive(false);
        }
    }

    public void OpenAnimPanel(GameObject target)
    {
        animChoose.SetActive(true);
        characterChoose.SetActive(false);

        foreach (GameObject panel in animPanels)
        {
            GameObject selectedPanel = target;

            panel.SetActive(false);
            selectedPanel.SetActive(true);
        }
    }
}
