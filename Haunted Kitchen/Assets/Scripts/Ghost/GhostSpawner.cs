using Unity.VisualScripting;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    [Header("Ghost")]
    [SerializeField] private GhostController ghostPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    //[SerializeField] private GhostStartBehavior[] possibleStartState; <= Uncomment if ghost state is random after spawn

    [Header("Rules")]
    [SerializeField] private bool spawnOnStart = true;
    [SerializeField] private float spawnCooldown = 0f;

    [SerializeField] private bool canSpawn = true;
    private GhostController currentGhost;

    public enum GhostStartBehavior
    {
        Idle,
        PourOil,
        //TurnOffLight
    }

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
        currentGhost = Instantiate(
            ghostPrefab,
            spawnPoint != null ? spawnPoint.position : transform.position,
            spawnPoint != null ? spawnPoint.rotation : Quaternion.identity
            );

        currentGhost.SetInitialState(GhostStartBehavior.Idle);

        currentGhost.OnGhostDestroyed += () =>
        {
            if (this == null) return; //Prevent calling while spawner is disabled

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
        };

        // ----- Random ghost state after spawn -----
        //GhostStartBehavior startState = GetRandomStartState();
        //ghost.SetInitialState(startState);
        // ------------------------------------------
    }

    //private GhostStartBehavior GetRandomStartState()
    //{
    //    if (possibleStartState == null || possibleStartState.Length == 0)
    //        return GhostStartBehavior.Idle;

    //    int index = Random.Range(0, possibleStartState.Length);
    //    return possibleStartState[index];
    //}

    private void ResetSpawn()
    {
        canSpawn = true;
        TrySpawn();
    }
}
