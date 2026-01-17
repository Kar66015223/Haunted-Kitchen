using UnityEngine;

[CreateAssetMenu(
    fileName = "NewIngredientData",
    menuName = "Game Data/Items/Ingredient"
    )]

public class IngredientData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int price;
    public GameObject cookedIngredientPrefab;
}
