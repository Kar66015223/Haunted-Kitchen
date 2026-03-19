using System.Collections.Generic;
using UnityEngine;

public class GhostStateFactory
{
    public IGhostState CreateState(
        GhostStartBehavior behavior,
        GhostController controller,
        LightSwitch lightSwitch,
        List<IDestroyable> destroyTargets,
        GameObject oilPrefab,
        GameObject oilCup)
    {
        return behavior switch
        {
            GhostStartBehavior.Idle => new GhostIdleState(controller),
            GhostStartBehavior.PourOil => new GhostPourOilState(controller, oilPrefab, oilCup),
            GhostStartBehavior.TurnOffLight => new GhostTurnOffLightState(controller, lightSwitch),
            GhostStartBehavior.Destroy => new GhostDestroyState(controller, destroyTargets),
            GhostStartBehavior.Possess => new GhostPossessState(controller),
            _ => new GhostIdleState(controller)
        };
    }
}
