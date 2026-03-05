using UnityEngine;

public class GhostPosessState : IGhostState
{

    private GhostController controller;
    private GhostPosessController posessController;

    [SerializeField] private float lookTime = 3f;

    public GhostPosessState(GhostController controller)
    {
        this.controller = controller;
        posessController = controller.GetComponent<GhostPosessController>();
    }

    public void Enter()
    {
        Debug.Log("Ghost enters Posess state");
    }

    public void Update()
    {
        
    }

    public void Exit()
    {
        Debug.Log("Ghost exits Posess state");
        controller.Disappear();
    }
}
