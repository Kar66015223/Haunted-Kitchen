using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GhosrDestroyController : MonoBehaviour
{
    [SerializeField] private List<IDestroyable> allTargets = new();

    private void Start()
    {
        allTargets = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDestroyable>()
            .ToList();
    }

    public IDestroyable PickRandomTarget()
    {
        if (allTargets == null || allTargets.Count == 0)
            return null;

        var validTargets = allTargets
            .Where(t => t.Status == StationStatus.Usable)
            .ToList();

        if (validTargets.Count == 0)
            return null;

        int randomIndex = Random.Range(0, validTargets.Count);
        return validTargets[randomIndex];
    }

    public void Attack(IDestroyable target)
    {
        target.SetStationStatus(StationStatus.Destroyed);
    }
}
