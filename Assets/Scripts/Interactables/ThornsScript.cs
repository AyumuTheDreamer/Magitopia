using UnityEngine;

public class ThornsScript : MonoBehaviour
{
    public GameObject particleEffectPrefab; // Assign this in the inspector
    public InventoryManager inventoryManager; // Assign this in the inspector
    public string requiredItemName = "Toxic Potion"; // Name of the required item
    public string messageForPlayer = "You need a strong poison to get rid of the thorns.";
    public SoundManager soundManager;
    public void Interact()
    {
        if (inventoryManager.HasItem(requiredItemName))
        {
            ClearThorns();
            soundManager.PlayDissolve();
        }
        else
        {
            Debug.Log(messageForPlayer); // Replace with your UI message system
        }
    }

    private void ClearThorns()
    {
        // Play particle effect
        Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);

        // Find and remove the item from inventory
        Item itemToRemove = inventoryManager.Items.Find(item => item.itemName == requiredItemName);
        if (itemToRemove != null)
        {
            itemToRemove.quantity--;
            if (itemToRemove.quantity <= 0)
            {
                inventoryManager.Remove(itemToRemove);
            }
            // Optionally update UI here if needed
        }

        // Destroy or disable the thorn obstacle
        Destroy(gameObject);
    }

    
   
}
