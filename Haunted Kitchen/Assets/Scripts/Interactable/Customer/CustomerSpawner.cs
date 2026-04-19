using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<DayConfiguration> dayConfigs;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private List<Table> tables = new();
    private DayConfiguration currentConfig;
    private Coroutine spawnLoop;

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartSpawning;
        DayManager.Instance.OnDayStarted += SetupDay;
    }

    void OnDisable()
    {
        GameEvents.OnGameStart -= StartSpawning;

        if (DayManager.Instance != null)
            DayManager.Instance.OnDayStarted -= SetupDay;
    }

    private void SetupDay(int dayNumber)
    {
        // Day 1 = index 0
        int index = Mathf.Clamp(dayNumber - 1, 0, dayConfigs.Count - 1);
        currentConfig = dayConfigs[index];

        currentConfig.ResetCounts();

        FindTables();
    }

    private void StartSpawning()
    {
        if (spawnLoop != null)
            StopCoroutine(spawnLoop);

        spawnLoop = StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (currentConfig == null) yield return null;

        while (true)
        {
            if (HasFreeTable() && TryGetConfig(out CustomerConfiguration config))
            {
                Spawn(config.prefab);
                config.currentSpawnCount++;
            }

            yield return new WaitForSeconds(currentConfig.spawnInterval);
        }
    }

    private void FindTables()
    {
        tables.Clear();

        tables = FindObjectsByType<Table>(FindObjectsSortMode.None)
        .Where(t => t.Chairs.Length != 0)
        .ToList();
    }

    private bool HasFreeTable() => tables.Any(t => t.HasFreeChair());

    private bool TryGetConfig(out CustomerConfiguration result)
    {
        var availablePool = currentConfig.customerPool
            .Where(p => p.spawnLimits <= 0 || p.currentSpawnCount < p.spawnLimits)
            .ToList();

        if (availablePool.Count > 0)
        {
            result = availablePool[Random.Range(0, availablePool.Count)];
            return true;
        }

        result = null;
        return false;
    }
    
    private void Spawn(GameObject prefab)
    {
        GameObject customerObj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        CustomerMovement movement = customerObj.GetComponentInParent<CustomerMovement>();

        movement.Initialize(tables, spawnPoint);
    }
}