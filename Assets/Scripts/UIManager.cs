using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject craftingPopup;
    public GameObject extendedInventoryPanel;

    // Function to toggle the visibility of the extended inventory
    public void ToggleExtendedInventory()
    {
        if (extendedInventoryPanel != null)
        {
            extendedInventoryPanel.SetActive(!extendedInventoryPanel.activeSelf);
            if(extendedInventoryPanel.activeSelf)
            {
                Time.timeScale = 0; 
            }
            else
            {
                if(!craftingPopup.activeSelf)
                {
                    Time.timeScale = 1; 
                }
            }
        }
    }
}
