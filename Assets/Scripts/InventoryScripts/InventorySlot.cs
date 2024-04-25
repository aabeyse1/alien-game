using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Outline outline;
    private bool isEquipped = false;
    private ItemRepresentation itemRep;

    public CharacterEquipManager equipManager;

    void Start()
    {
        itemRep = GetComponentInChildren<ItemRepresentation>(true);
        SetEquipped(false); // Start with no outline
    }

    public void OnSlotClicked()
    {
        itemRep = GetComponentInChildren<ItemRepresentation>();
        ToggleEquipItem();
    }

    private void ToggleEquipItem()
    {
        if (itemRep != null && itemRep.gameObject.activeSelf)
        {
            SetEquipped(!isEquipped);
        }
    }

    public void SetEquipped(bool equip)
    {
        isEquipped = equip;
        outline.enabled = isEquipped;
        equipManager.UpdateEquippedItem(this, isEquipped ? itemRep.item : null);
    }

    public string GetItemName() {
        return itemRep.item.itemName;
    }
}
