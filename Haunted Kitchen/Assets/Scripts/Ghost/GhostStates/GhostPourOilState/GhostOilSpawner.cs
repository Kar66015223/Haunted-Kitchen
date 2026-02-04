using UnityEngine;

public class GhostOilSpawner : MonoBehaviour
{
    public GameObject oilPrefab;
    public float forwardOffset = 1f;

    public void PourOil()
    {
        if (oilPrefab == null)
        {
            Debug.LogError("no oil prefab, can't pour oil");
            return;
        }

        Vector3 spawnPos = transform.position + transform.forward * forwardOffset;
        Instantiate(oilPrefab, spawnPos, Quaternion.identity);
    }
}
