using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> items = new List<Item>();

    public Item GetItemByName(string itemName)
    {
        return items.Find(item => item.itemName == itemName);
    }
}
