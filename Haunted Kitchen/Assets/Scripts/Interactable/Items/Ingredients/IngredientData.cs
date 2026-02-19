using UnityEngine;

[CreateAssetMenu(
    fileName = "NewIngredientData",
    menuName = "Game Data/Items/Ingredient"
    )]

public class IngredientData : ItemData
{
    public bool isCookable;
    public float cookTime;

    public CookingMethod method;

    public ItemData cookedResult;
    public Material cookedMaterial;

    public GameObject cookedModel;
}
