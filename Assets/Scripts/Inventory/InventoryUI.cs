using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    // Reference to the PlayerMovement script
    public PlayerMovement playerMovement;
    public ThirdPersonCam thirdPersonCam;
    public InventoryManager inventoryManager;
    public PauseMenu pauseMenu;
    // Update is called once per frame
    void Update()
    {
        // Check if the 'I' key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
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
                thirdPersonCam.LockCameraOrientation();
            }
            else
            {
                // Re-lock the cursor when the inventory is closed
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                thirdPersonCam.UnlockCameraOrientation();
            }
            inventoryManager.ToggleInventory(inventoryPanel.activeSelf);
        }
    }
}
