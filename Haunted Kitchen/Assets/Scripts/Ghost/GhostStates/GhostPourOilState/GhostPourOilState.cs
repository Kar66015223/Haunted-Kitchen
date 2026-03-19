using UnityEngine;
using UnityEngine.AI;

public class GhostPourOilState : IGhostState
{
    private GhostController controller;

    private GameObject oilPrefab;
    private GameObject oilCup;

    private float pourDuration = 1f;
    private float forwardOffset = 1.5f;

    private float timer;

    private bool actionPerformed = false;

    public GhostPourOilState(GhostController controller, GameObject oilPrefab, GameObject oilCup)
    {
        this.controller = controller;
        this.oilPrefab = oilPrefab;
        this.oilCup = oilCup;
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

        actionPerformed = false;

        timer = pourDuration;

        controller.Anim.SetTrigger(GhostConstants.ANIM_POUROIL);

        Debug.Log("Ghost enters PourOil state");
    }

    public void Update()
    {
        if (controller.HitRuneStone)
        {
            Exit();
            return;
        }
        
        if(!actionPerformed)
        {
            PourOil();
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Exit();
        }

        //Debug.Log("Ghost is in PourOil state");
    }

    public void Exit()
    {
        oilCup.SetActive(false);
        controller.Disappear();
        Debug.Log("Ghost is exiting PourOil state");
    }

    private void PourOil()
    {
        if (oilPrefab == null || actionPerformed)
        {
            Debug.LogError("no oil prefab or action performed");
            return;
        }

        oilCup.SetActive(true);

        Vector3 offsetPos = controller.transform.position + controller.transform.forward * forwardOffset;

        // ✅ NEW: Snap to NavMesh ground
        if (NavMesh.SamplePosition(offsetPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            Object.Instantiate(oilPrefab, hit.position, Quaternion.identity);
            actionPerformed = true;
        }
        else
        {
            Debug.LogWarning($"Could not find valid NavMesh position near {offsetPos}");
        }
    }
}
