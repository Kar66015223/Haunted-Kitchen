using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Ghost")]
    [SerializeField] private GhostController ghostPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    public GhostStartBehavior initialState = GhostStartBehavior.Idle;

    [Header("Rules")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private float spawnCooldown = 0f;

    [SerializeField] private bool canSpawn = true;
    private GhostController currentGhost;

    private void Start()
    {
        if (spawnOnStart)
        {
            TrySpawn();
        }
    }

    public void TrySpawn()
    {
        if (!canSpawn || ghostPrefab == null || currentGhost != null)
            return;

        SpawnGhost();
    }

    private void SpawnGhost()
    {
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        currentGhost = Instantiate(ghostPrefab, spawnPosition, spawnRotation);
        currentGhost.SetInitialState(initialState);

        currentGhost.OnGhostDestroyed += HandleGhostDestroyed;
    }

    private void HandleGhostDestroyed()
    {
        currentGhost = null;

        if (spawnCooldown > 0f)
        {
            canSpawn = false;
            Invoke(nameof(ResetSpawn), spawnCooldown);
        }
        else
        {
            TrySpawn();
        }
    }
    
    
    private void ResetSpawn()
    {
        canSpawn = true;
        TrySpawn();
    }
}
