using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> customerPrefabs;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private List<Table> tables = new();

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
        movement.Initialize(tables);

        movement.OnLeft += HandleCustomerLeft;
    }
}