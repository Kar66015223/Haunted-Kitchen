using UnityEngine;

public class GhostStateFactory
{
    public IGhostState CreateState(GhostStartBehavior behavior, GhostController controller)
    {
        return behavior switch
        {
            GhostStartBehavior.Idle => new GhostIdleState(controller),
            GhostStartBehavior.PourOil => new GhostPourOilState(controller),
            GhostStartBehavior.TurnOffLight => new GhostTurnOffLightState(controller),
            GhostStartBehavior.Destroy => new GhostDestroyState(controller),
            GhostStartBehavior.Possess => new GhostPossessState(controller),
            _ => new GhostIdleState(controller)
        };
    }
}
