using UnityEngine;

[CreateAssetMenu(
    fileName = "NewIngredientData",
    menuName = "Game Data/Items/Ingredient"
    )]

public class IngredientData : ItemData
{
    public int price;
    public CookedIngredientData cookedResult;
    public Material cookedMaterial;
}
