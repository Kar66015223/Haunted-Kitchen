using UnityEngine;

public class RuneSpawner : MonoBehaviour
{
    [SerializeField] private GameObject runePrefab;

    public void SpawnRune(Table currentTable)
    {
        if(currentTable.GetCurrentItem() != null)
            currentTable.SpawnItem(runePrefab);
    }
}
