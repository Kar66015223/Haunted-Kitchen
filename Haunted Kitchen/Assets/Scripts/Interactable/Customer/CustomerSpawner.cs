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

    public int ActiveCustomerCount { get; private set; }

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartSpawning;
        DayManager.Instance.OnDayStarted += SetupDay;
        SubscribeToTimer();
    }

    void OnDisable()
    {
        GameEvents.OnGameStart -= StartSpawning;

        if (DayManager.Instance != null)
        {
            DayManager.Instance.OnDayStarted -= SetupDay;

            if (DayManager.Instance.Timer != null)
                DayManager.Instance.Timer.OnTimerRunOut -= StopSpawning;
        }
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

    private void StopSpawning()
    {
        if(spawnLoop != null)
        {
            StopCoroutine(spawnLoop);
            spawnLoop = null;
        }
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

        Customer_New customer = customerObj.GetComponent<Customer_New>();
        CustomerMovement movement = customerObj.GetComponentInParent<CustomerMovement>();

        if (customer != null && movement != null)
        {
            ActiveCustomerCount++;
            movement.OnLeft += HandleCustomerFinished;
        }

        movement.Initialize(tables, spawnPoint);
    }

    private void HandleCustomerFinished()
    {
        ActiveCustomerCount--;
        ActiveCustomerCount = Mathf.Max(0, ActiveCustomerCount);
    }
    
    public void SubscribeToTimer()
    {
        if (DayManager.Instance?.Timer != null)
        {
            DayManager.Instance.Timer.OnTimerRunOut -= StopSpawning;
            DayManager.Instance.Timer.OnTimerRunOut += StopSpawning;
        }
    }
}