using UnityEngine;

public interface IContainerInteractable
{
    bool HandleContainerInteraction(GameObject interactor, Table table);
}
