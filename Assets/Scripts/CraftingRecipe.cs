using UnityEngine;


[CreateAssetMenu(fileName = "NewRecipe", menuName = "Inventory/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    public Item[] requiredItems;
    public Item resultItem;
}
