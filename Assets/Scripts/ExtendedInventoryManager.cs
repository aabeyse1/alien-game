using UnityEngine;
using UnityEngine.UI;

public class ExtendedInventoryManager : MonoBehaviour
{
    public GameObject inventorySlotPrefab;
    public int slotCount = 20;

    public GameObject[] extendedInventorySlots;

    void Start()
    {
        CreateInventorySlots();
    }

    void CreateInventorySlots()
    {
        extendedInventorySlots = new GameObject[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, transform);
            extendedInventorySlots[i] = slot;
            // Optionally initialize each slot here...
        }
    }

    // Additional methods to manage items in the extended inventory...
}
