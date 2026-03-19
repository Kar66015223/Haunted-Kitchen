using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GhostDestroyState : IGhostState
{
    private GhostController controller;
    private List<IDestroyable> allTargets;
    private IDestroyable selectedTarget;

    private float hitDuration = 1f;
    private float timer;

    private bool actionPerformed = false;

    public GhostDestroyState(GhostController controller, List<IDestroyable> destroyTargets)
    {
        this.controller = controller;
        allTargets = destroyTargets;
    }

    public void Enter()
    {
        var validTargets = allTargets
            .Where(t => t.Status == StationStatus.Usable)
            .ToList();

        if (validTargets.Count == 0)
        {
            Exit();
            return;
        }

        selectedTarget = validTargets[Random.Range(0, validTargets.Count)];
 
        MonoBehaviour mono = selectedTarget as MonoBehaviour;
        if (mono != null)
        {
            Vector3 offset = mono.transform.forward * 1.5f;
            Vector3 teleportPosition = mono.transform.position + offset;
 
            controller.TeleportTo(teleportPosition);
            controller.transform.LookAt(mono.transform.position);
            controller.Movement.Stop();
 
            controller.Anim.SetTrigger(GhostConstants.ANIM_ATTACK);
        }

        timer = hitDuration;
        actionPerformed = false;

        Debug.Log("Ghost enters Destroy state");
    }
    public void Update()
    {
        if (controller.HitRuneStone)
        {
            Exit();
            return;
        }
        
        if (!actionPerformed)
        {
            var target = allTargets.FirstOrDefault(t => t.Status == StationStatus.Usable);
            if (target != null)
            {
                target.SetStationStatus(StationStatus.Destroyed);
                actionPerformed = true;

                Debug.Log($"Ghost destroys {target}");
            }
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Exit();
        }
    }

    public void Exit()
    {
        Debug.Log("Ghost exits Destroy state");
        controller.Disappear();
    }
}
