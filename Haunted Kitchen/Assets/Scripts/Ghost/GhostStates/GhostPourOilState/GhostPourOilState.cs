using Unity.VisualScripting;
using UnityEngine;

public class GhostPourOilState : IGhostState
{
    private GhostController controller;
    private GhostOilSpawner oilSpawner;

    public float pourDuration = 1f;
    private float timer;

    public GhostPourOilState(GhostController controller)
    {
        this.controller = controller;
        oilSpawner = controller.GetComponent<GhostOilSpawner>();
    }

    public void Enter()
    {
        if (NavMeshUtility.TryGetRandomPoint(
            controller.transform.position,
            20f,
            out Vector3 spawnPos))
        {
            //Debug.Log($"NavMesh point found: {spawnPos}");
            controller.TeleportTo(spawnPos);

            if (NavMeshUtility.TryGetRandomPoint(
            controller.transform.position,
            5f,
            out Vector3 lookTarget))
            {
                Vector3 dir = (lookTarget - controller.transform.position).WithY(0);

                if (dir.sqrMagnitude > 0.01f)
                    controller.transform.rotation = Quaternion.LookRotation(dir);
            }
        }

        else
        {
            Debug.LogWarning("NavMesh random point FAILED");
        }

        oilSpawner.PourOil();

        timer = pourDuration;

        Debug.Log("Ghost enters PourOil state");
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            controller.Disappear();
        }

        //Debug.Log("Ghost is in PourOil state");
    }

    public void Exit()
    {
        Debug.Log("Ghost is exiting PourOil state");
    }
}
