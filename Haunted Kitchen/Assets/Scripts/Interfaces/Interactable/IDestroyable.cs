using UnityEngine;

public interface IDestroyable
{
    StationStatus Status { get; }
    void SetStationStatus(StationStatus newStatus);
}
