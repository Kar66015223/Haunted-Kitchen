using UnityEngine;

public interface ITableInteractable
{
    /// <summary>
    /// Returns true if the interaction was handled.
    /// </summary>
    bool HandleTableInteraction(GameObject interactor);
}
