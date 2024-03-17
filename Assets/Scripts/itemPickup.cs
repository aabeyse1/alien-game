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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    void Pickup()
    {
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        bool wasAdded = inventoryManager.AddItem(item);
        if (wasAdded)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Inventory is full.");
        }
    }
}
