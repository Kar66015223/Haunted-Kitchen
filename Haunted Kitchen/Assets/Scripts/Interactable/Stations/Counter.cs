using UnityEngine;

public class Counter : Table
{
    public GameObject KetchupBottlePrefab;
    public GameObject MustardBottlePrefab;

    private void Awake()
    {
        tableRole = TableRole.Counter;
    }

    private void Start()
    {
        if (KetchupBottlePrefab != null)
            SpawnItem(KetchupBottlePrefab);

        if (MustardBottlePrefab != null)
            SpawnItem(MustardBottlePrefab);
    }
}
