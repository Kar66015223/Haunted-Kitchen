using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public List<RecipeStep> steps;
    public ItemData result;
}
