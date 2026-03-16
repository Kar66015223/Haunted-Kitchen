using UnityEngine;

[System.Serializable]
public class SitVisual
{
    [SerializeField] private string behaviorTypeName;
    public GameObject visual;

    public System.Type BehaviorType
    {
        get
        {
            if (string.IsNullOrEmpty(behaviorTypeName))
                return null;
            return System.Type.GetType(behaviorTypeName);
        }
    }

    public void SetBehaviorType(System.Type type)
    {
        behaviorTypeName = type?.FullName ?? "";
    }
    
#if UNITY_EDITOR
    public string BehaviorTypeDisplay => string.IsNullOrEmpty(behaviorTypeName) ? "None" : behaviorTypeName.Split('.')[^1];
#endif
}
