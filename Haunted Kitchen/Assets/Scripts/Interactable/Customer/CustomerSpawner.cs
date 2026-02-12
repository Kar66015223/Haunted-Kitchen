using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
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
            Debug.LogError("No tables assigned in inspector");
            return;
        }

        //pick random free table
        List<Table> freeTables = tables.FindAll(tables => !tables.isOccupied);

        if (freeTables.Count == 0)
        {
            Debug.Log("No free tables");
            return;
        }

        Table targetTable = freeTables[Random.Range(0, freeTables.Count)];

        GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        Customer customerComponent = customerObj.GetComponent<Customer>();
        customerComponent.OnCustomerLeft += HandleCustomerLeft;

        Customer customer = customerObj.GetComponent<Customer>();
        if (customer == null)
        {
            Debug.LogError("Customer Prefab is missing Customer script");
            return;
        }

        customer.targetTable = targetTable;
        customer.standPoint = targetTable.customerStandPoint;
        customer.targetTable.isOccupied = true;

        customer.SetExitPoint(spawnPoint);

        NavMeshAgent agent = customer.agent;
        agent.SetDestination(customer.standPoint.position);
    }
}