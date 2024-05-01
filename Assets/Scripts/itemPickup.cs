using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    private bool isPlayerNearby = false;

    [SerializeField] GameObject enterTutorialObject;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.Return))
        {

            Pickup();
            // Tutorial won't show up anymore
            if (!TutorialManager.instance.hasPickedUpItem)
            {
                TutorialManager.instance.hasPickedUpItem = true;
                if (enterTutorialObject)
                {
                    enterTutorialObject.SetActive(false);
                }
            }

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = true;
            // increase the size of the item when you walk past it
            transform.localScale = transform.localScale * 1.1f;
            if (!TutorialManager.instance.hasPickedUpItem)
            {
                // if haven't poked yet, show the spacebar icon telling you how to use tools
                enterTutorialObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNearby = false;
            // reset the size of the item
            transform.localScale = transform.localScale / 1.1f;
            if (enterTutorialObject)
            {
                enterTutorialObject.SetActive(false);
            }

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
             DialogueManager.instance.RunDialogueNode("Inventory_Full");
            Debug.Log("Inventory is full.");
        }
    }
}
