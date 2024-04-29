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

    void Awake()
    {
        itemRep = GetComponentInChildren<ItemRepresentation>(true);
    }

    void Update()
    {
        itemRep = GetComponentInChildren<ItemRepresentation>(true);
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
        if (equip) {
             GameEventsManager.instance.pickUpEvents.ItemEquipped();
        }
        equipManager.UpdateEquippedItem(this, isEquipped ? itemRep.item : null);

       
    }

    public string GetItemName() {
        return itemRep.item.itemName;
    }
}
