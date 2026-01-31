using UnityEngine;

[CreateAssetMenu(
    fileName = "NewItemData",
    menuName = "Game Data/Items/Item"
    )]

public class ItemData : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite icon;
    public bool oneHand;
}
