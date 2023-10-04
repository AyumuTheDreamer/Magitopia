using UnityEngine;

public class CropInteraction : MonoBehaviour
{
    public HarvestableCrop crop; // Reference to the specific crop type 
    public Animator animator;
    public InventoryManager inventoryManager;

    public int cropQuantity;

    public void Harvest()
    {
        animator.SetTrigger("Interact");
        Debug.Log("Harvesting the crop: " + gameObject.name);

        if (inventoryManager != null)
        {
            // Create a new instance of the Item Scriptable Object
            Item cropItem = ScriptableObject.CreateInstance<Item>();
            cropItem.id = crop.cropName;
            cropItem.itemName = crop.cropName;
            cropItem.quantity = cropQuantity;
            cropItem.isStackable = true; 
            cropItem.maxStackCount = 99; // Set an appropriate maximum stack count

            // Add the crop item to the inventory
            inventoryManager.Add(cropItem);
        }
    }
}
