using UnityEngine;
using UnityEngine.UI;

public class ExtendedInventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public int slotCount = 20;
    public Transform slotsParent;
    public GameObject[] extendedInventorySlots;

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
            extendedInventorySlots[i] = slot;
        }
    }
}
