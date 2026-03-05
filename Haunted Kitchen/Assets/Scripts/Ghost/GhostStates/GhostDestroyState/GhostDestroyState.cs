using UnityEngine;

public class GhostDestroyState : IGhostState
{
    private GhostController controller;
    private GhosrDestroyController destroyController;

    private float hitDuration = 1f;
    private float timer;

    public GhostDestroyState(GhostController controller)
    {
        this.controller = controller;
        destroyController = controller.GetComponent<GhosrDestroyController>();
    }

    public void Enter()
    {
        IDestroyable target = destroyController.PickRandomTarget();
        if (target == null)
        {
            Exit();
            return;
        }

        MonoBehaviour mono = target as MonoBehaviour;
        if (mono != null)
        {
            Vector3 offset = mono.transform.forward * 1.5f;
            Vector3 teleportPosition = mono.transform.position + offset;

            controller.TeleportTo(teleportPosition);
            controller.transform.LookAt(mono.transform.position);

            destroyController.Attack(target);
            controller.anim.SetTrigger("Attack");
        }

        timer = hitDuration;

        Debug.Log("Ghost enters Destroy state");
    }
    public void Update()
    {
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
