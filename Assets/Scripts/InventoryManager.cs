using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots; // Your inventory slots

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

                // Scale the image to be 50% of its size
                itemIconTransform.localScale = new Vector3(0.7f, 0.7f, 1);

                return true; // Item was successfully added to the first available slot
            }
        }
        return false; // If no slot was available (all were active), return false
    }
}
