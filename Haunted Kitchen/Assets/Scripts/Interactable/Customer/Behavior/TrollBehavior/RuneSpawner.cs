using System.Security.Cryptography;
using UnityEngine;

public class RuneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject runePrefab;

    public void SpawnRune(Table currentTable)
    {
        currentTable.SpawnItem(runePrefab);
    }
}
