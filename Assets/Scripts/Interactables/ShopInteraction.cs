using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteraction : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject inventoryPanel;
    public InventoryManager inventoryManager;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
    }

    public void Interact()
    {
        Debug.Log("Interact method called on " + gameObject.name);

        shopPanel.SetActive(!shopPanel.activeSelf);
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        playerMovement.isInventoryOpen = inventoryPanel.activeSelf;

        if (inventoryPanel.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        
    }

}
