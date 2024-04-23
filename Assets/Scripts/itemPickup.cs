using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Return))
        {
            
            Pickup();

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // increase the size of the item when you walk past it
            transform.localScale = transform.localScale * 1.1f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
             // reset the size of the item
            transform.localScale = transform.localScale / 1.1f;
        }
    }

    void Pickup()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        bool wasAdded = inventoryManager.AddItem(item, null);
        if (wasAdded)
        {
            
            GameEventsManager.instance.pickUpEvents.ItemPickedUp(item);
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Inventory is full.");
        }
    }
}
