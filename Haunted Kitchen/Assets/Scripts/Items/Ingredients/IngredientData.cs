using UnityEngine;

[CreateAssetMenu(
    fileName = "NewIngredientData",
    menuName = "Game Data/Items/Ingredient"
    )]

public class IngredientData : ItemData
{
    public int price;

    public bool isCookable;

    public IngredientData cookedResult;
    public Material cookedMaterial;
}
