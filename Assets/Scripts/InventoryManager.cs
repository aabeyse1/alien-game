using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public Sprite stickSprite; 

    public bool AddItem(Item item)
    {
        foreach (GameObject slot in inventorySlots)
        {
            Transform itemIconTransform = slot.transform.GetChild(0);
            Image itemImage = itemIconTransform.GetComponent<Image>();

            if (!itemIconTransform.gameObject.activeInHierarchy)
            {
                itemIconTransform.gameObject.SetActive(true);
                itemImage.sprite = item.itemSprite; 
                return true;
            }
        }
        return false;
    }

}
