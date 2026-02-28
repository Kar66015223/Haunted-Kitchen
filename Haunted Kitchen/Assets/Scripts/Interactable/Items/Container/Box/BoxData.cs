using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewIngredientData",
    menuName = "Game Data/Items/Box"
    )]

public class BoxData : ItemData
{
    public int maxAmount = 20;
    public GameObject content;
}
