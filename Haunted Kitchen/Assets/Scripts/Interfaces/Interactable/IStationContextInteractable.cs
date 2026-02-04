using UnityEngine;

public interface IStationContextInteractable
{
    bool CanStationInteract(PlayerItem playerItem);
    bool CanContainerInteract(ContainerItem container);
    void HandleContainer(ContainerItem container, Table table);
}
