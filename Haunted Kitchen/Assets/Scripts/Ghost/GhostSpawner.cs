using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GhostSpawner : MonoBehaviour
{
    [Header("Ghost")]
    [SerializeField] private GhostController ghostPrefab;

    [Header("Spawn")]
    [SerializeField] private Transform spawnPoint;
    public GhostStartBehavior initialState = GhostStartBehavior.Idle;
    [SerializeField] private UniversalRendererData fullScreenFX;

    [Header("Rules")]
    [SerializeField] private bool spawnOnStart = true;
    public bool allowSpawn = true;
    [SerializeField] private float spawnCooldown = 0f;

    [SerializeField] private bool canSpawn = true;
    private GhostController currentGhost;

    void OnEnable()
    {
        GameEvents.OnToggleGhostSpawning += SetAllowSpawn;
    }

    void OnDisable()
    {
        GameEvents.OnToggleGhostSpawning -= SetAllowSpawn;

        if (currentGhost != null)
            currentGhost.OnGhostDestroyed -= HandleGhostDestroyed;

        if (fullScreenFX != null)
            SetScreenFX(false);
    }

    private void Start()
    {
        if (spawnOnStart)
        {
            TrySpawn();
        }

        if (fullScreenFX != null)
            SetScreenFX(false);
    }

    void Update()
    {
        if(allowSpawn && canSpawn)
        {
            TrySpawn();
        }
    }

    public void TrySpawn()
    {
        if (!allowSpawn) return;

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
        if (!allowSpawn) return;

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

    private void SetAllowSpawn(bool value)
    {
        allowSpawn = value;
        SetScreenFX(allowSpawn);
    }
    
    private void SetScreenFX(bool value)
    {
        foreach (var feature in fullScreenFX.rendererFeatures)
        {
            if (feature.name == "Rush Hour")
            {
                feature.SetActive(value);
            }
        }
    }
}
