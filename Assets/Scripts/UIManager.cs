using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject extendedInventoryPanel; // Assign this in the inspector

    // Function to toggle the visibility of the extended inventory
    public void ToggleExtendedInventory()
    {
        if (extendedInventoryPanel != null)
        {
            extendedInventoryPanel.SetActive(!extendedInventoryPanel.activeSelf);
        }
    }
}
