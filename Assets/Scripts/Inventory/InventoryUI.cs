using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    // Reference to the PlayerMovement script
    public PlayerMovement playerMovement;

    public InventoryManager inventoryManager;

    // Update is called once per frame
    void Update()
    {
        // Check if the 'I' key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Toggle the Inventory UI panel
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);

            // Update the isInventoryOpen variable in the PlayerMovement script
            playerMovement.isInventoryOpen = inventoryPanel.activeSelf;
            inventoryManager.ListItems();

            // Lock or unlock the cursor based on the inventory state
            if (inventoryPanel.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Re-lock the cursor when the inventory is closed
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            inventoryManager.ToggleInventory(inventoryPanel.activeSelf);
        }
    }
}
