using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DayConfiguration", menuName = "Game Data/DayConfiguration")]
public class DayConfiguration : ScriptableObject
{
    public float spawnInterval = 30f;
    public List<CustomerConfiguration> customerPool;

    public void ResetCounts()
    {
        foreach(var config in customerPool)
        {
            config.currentSpawnCount = 0;
        }
    }
}
