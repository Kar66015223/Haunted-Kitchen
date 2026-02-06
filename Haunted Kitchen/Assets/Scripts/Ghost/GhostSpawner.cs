using Unity.VisualScripting;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Ghost")]
    [SerializeField] private GhostController ghostPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GhostStartBehavior[] possibleStartState;

    [Header("Rules")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private float spawnCooldown = 0f;

    [SerializeField] private bool canSpawn = true;

    public enum GhostStartBehavior
    {
        Idle,
        PourOil,
        TurnOffLight
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            TrySpawn();
        }
    }

    //private void Update()
    //{
    //    if (canSpawn)
    //    {
    //        TrySpawn();
    //    }
    //}

    public void TrySpawn()
    {
        if (!canSpawn || ghostPrefab == null)
            return;

        SpawnGhost();

        if (spawnCooldown > 0f)
        {
            canSpawn = false;
            Invoke(nameof(ResetSpawn), spawnCooldown);
        }
    }

    private void SpawnGhost()
    {
        GhostController ghost = Instantiate(
            ghostPrefab,
            spawnPoint != null ? spawnPoint.position : transform.position,
            spawnPoint != null ? spawnPoint.rotation : Quaternion.identity
            );

        GhostStartBehavior startState = GetRandomStartState();
        ghost.SetInitialState(startState);
    }

    private GhostStartBehavior GetRandomStartState()
    {
        if (possibleStartState == null || possibleStartState.Length == 0)
            return GhostStartBehavior.Idle;

        int index = Random.Range(0, possibleStartState.Length);
        return possibleStartState[index];
    }

    private void ResetSpawn()
    {
        canSpawn = true;
    }
}
