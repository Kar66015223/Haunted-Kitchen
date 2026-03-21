using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> customerPrefabs;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private List<Table> tables = new();

    private void OnEnable()
    {
        tables.Clear();

        tables = FindObjectsByType<Table>(FindObjectsSortMode.None)
        .Where(t => t.Chairs.Length != 0)
        .ToList();

        Debug.Log($"CustomerSpawner found {tables.Count} tables");
    }

    private void Start()
    {
        SpawnCustomer();
    }

    void HandleCustomerLeft()
    {
        SpawnCustomer();
    }

    public void SpawnCustomer()
    {
        if (tables.Count == 0)
        {
            Debug.LogError("No tables assigned");
            return;
        }

        if (customerPrefabs.Count == 0)
        {
            Debug.LogError("No customer prefabs assigned");
            return;
        }

        GameObject randomPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Count)];
        GameObject customerObj = Instantiate(randomPrefab, spawnPoint.position, spawnPoint.rotation);

        CustomerMovement movement = customerObj.GetComponentInParent<CustomerMovement>();
        movement.Initialize(tables, spawnPoint);

        movement.OnLeft += HandleCustomerLeft;
    }
}