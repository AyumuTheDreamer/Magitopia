using UnityEngine;

public class CropInteraction : MonoBehaviour
{
    public HarvestableCrop crop; // Reference to the specific crop type 
    public Animator animator;
    public InventoryManager inventoryManager;
    public float growthTime = 10f; // Adjust this value based on your desired growth time
    private float currentGrowthTime = 0f;
    private bool isFullyGrown = false;
    public GameObject immatureModel;
    public GameObject fullyGrownModel;
    public int harvestedQuantity = 5;

   private void Update()
    {
        if (!isFullyGrown)
        {
            currentGrowthTime += Time.deltaTime;
            if (currentGrowthTime >= growthTime)
            {
                isFullyGrown = true;
                // Activate the fully grown model and deactivate the immature model
                immatureModel.SetActive(false);
                fullyGrownModel.SetActive(true);
            }
        }
    }

public bool IsFullyGrown()
{
    return isFullyGrown;
}

 public void Harvest()
{
    animator.SetTrigger("Interact");
    Debug.Log("Harvesting the crop: " + gameObject.name);

    if (inventoryManager != null)
    {
        // Create a new instance of the Item Scriptable Object
        Item cropItem = ScriptableObject.CreateInstance<Item>();
        cropItem.itemName = crop.cropName;

        // Determine the harvested quantity based on growth status
        if (isFullyGrown)
        {
            cropItem.quantity = harvestedQuantity;
            Debug.Log("Harvesting fully grown crop. Quantity: " + harvestedQuantity);
        }
        else
        {
            cropItem.quantity = 0;
            Debug.Log("Harvesting immature crop. Quantity: 0");
        }

        cropItem.isStackable = true;
        cropItem.maxStackCount = 99;

        // Reset the growth timer and state
        currentGrowthTime = 0f;
        isFullyGrown = false;  // Reset isFullyGrown to false

        // Activate the immature model and deactivate the fully grown model
        immatureModel.SetActive(true);
        fullyGrownModel.SetActive(false);

        // Add the crop item to the inventory
        inventoryManager.Add(cropItem);
    }
}
}