using UnityEngine;
using UnityEngine.UI;

public class SkipToEnding : MonoBehaviour
{
    [SerializeField] private Button goodEnd;
    [SerializeField] private Button badEnd;

    void Start()
    {
        goodEnd.onClick.AddListener(ToGoodEnd);
        badEnd.onClick.AddListener(ToBadEnd);
    }

    public void ToGoodEnd()
    {
        SceneLoader.ChangeScene(SceneConstants.ENDING_GOODEND_NAME);
    }
    
    public void ToBadEnd()
    {
        SceneLoader.ChangeScene(SceneConstants.ENDING_BADEND_NAME);
    }
}
