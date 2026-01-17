using UnityEngine;

public class Griddle : Kitchenware
{
    private void Start()
    {
        kitchenWareName = "Griddle";
        kitchenwareStatus = KitchenwareStatus.Usable;
    }

    public override void Use()
    {
        base.Use();
    }
}
