using UnityEngine;

public class SeedGiver : MonoBehaviour
{
    public Seeds seedToGive; // Reference to the seed you want to give to the player.
    public InventoryManager inventoryManager; // Reference to the player's inventory manager.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Check if the player's inventory manager is available.
            if (inventoryManager != null)
            {
                // Create a new item using the seed reference and add it to the player's inventory.
                Item newItem = new Item
                {
                    id = seedToGive.id,
                    itemName = seedToGive.itemName,
                    itemIcon = seedToGive.itemIcon,
                    isStackable = seedToGive.isStackable,
                    quantity = 1 // You can adjust the quantity as needed.
                };

                inventoryManager.Add(newItem);

                // Optionally, you can display a message to indicate that the player received the seed.
                Debug.Log("Received: " + seedToGive.itemName);

                // Disable the object to prevent the player from receiving the seed multiple times.
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("Inventory manager is missing. Ensure it's assigned in the inspector.");
            }
        }
    }
}
