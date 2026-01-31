using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void SpawnCustomer()
    {
        if (tables.Count == 0)
        {
            Debug.LogError("No tables assigned in inspector");
            return;
        }

        //pick random free table
        Table targetTable = tables[Random.Range(0, tables.Count)];

        GameObject customerObj = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        Customer customer = customerObj.GetComponent<Customer>();
        if (customer == null)
        {
            Debug.LogError("Customer Prefab is missing Customer script");
            return;
        }

        customer.targetTable = targetTable;
        customer.standPoint = targetTable.customerStandPoint;
        customer.SetExitPoint(spawnPoint);

        NavMeshAgent agent = customer.agent;
        agent.SetDestination(customer.standPoint.position);
    }
}