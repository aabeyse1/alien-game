using UnityEngine;
using UnityEngine.UI;

public class ExtendedInventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public int slotCount = 20;
    public Transform slotsParent;
    public GameObject[] extendedInventorySlots;
    public CraftingPopupManager craftingManager;
    public InventoryManager inventoryManager;

    void Awake()
    {
        CreateInventorySlots();
        gameObject.SetActive(false);
    }

    void CreateInventorySlots()
    {
        extendedInventorySlots = new GameObject[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, slotsParent);
            slot.name = "InventorySlot " + i;
            InventorySlot invSlot = slot.GetComponent<InventorySlot>();
            invSlot.inventoryManager = inventoryManager;
            // invSlot.craftingPopupManager = craftingManager;
            extendedInventorySlots[i] = slot;
            DraggableItem draggableItem = slot.GetComponentInChildren<DraggableItem>(true); // 'true' to include inactive children
            if (draggableItem != null)
            {
                draggableItem.inventoryManager = inventoryManager;
                draggableItem.craftingManager = craftingManager;
            }
        }
    }
}
