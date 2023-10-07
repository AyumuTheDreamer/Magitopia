using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AlchemyStationCrafting : MonoBehaviour
{
    public GameObject alchemyPanel;
    public GameObject inventoryPanel;
    public InventoryManager inventoryManager;
    private PlayerMovement playerMovement;
    public List<AlchemyRecipe> recipes;
    public AlchemyRecipe recipeToCraft;
private void Start()
    {
        // Find the PlayerMovement script in the scene
        playerMovement = FindObjectOfType<PlayerMovement>();
        
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement script not found in the scene!");
        }
    }



public virtual void Interact(AlchemyRecipe recipeToCraft = null)
{
    Debug.Log("Interact method called on " + gameObject.name);
    // Your existing interaction logic here

    alchemyPanel.SetActive(!alchemyPanel.activeSelf);
    inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    playerMovement.isInventoryOpen = inventoryPanel.activeSelf;

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

    if (recipeToCraft != null)
    {
        bool craftingResult = inventoryManager.CraftItem(recipeToCraft);
        if (craftingResult)
        {
            Debug.Log("Potion Made");
        }
        else
        {
            Debug.LogWarning("Crafting Failed");
        }
    }
}

}