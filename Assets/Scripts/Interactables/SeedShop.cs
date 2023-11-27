using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedShop : MonoBehaviour
{
    public Transform shopPanel;
    public GameObject shopItemButtonPrefab;
    public List<Seeds> seedItems;
    public CurrencyManager currencyManager;
    public GameObject shopUI;
    private bool isUIActive = false;
    private PlayerMovement playerMovement;
    public ThirdPersonCam thirdPersonCam;
    public GameObject firstTimeTextBox; // Add this line to hold the text box UI
    private bool isFirstTime = true;    // Add this line to track if it's the first time
    private bool isFirstTimeTextBoxActive = false;
    public Text shopCurrencyText;
    public Transform seedInventoryPanel; // Assign this in the inspector to your seed inventory panel in the shop UI
    public GameObject seedInventoryEntryPrefab;
    public Animator animator;
    public SoundManager soundManager;
    public ObjectiveManager objectiveManager;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        currencyManager = CurrencyManager.Instance;
        currencyManager.OnCurrencyChanged += UpdateButtonState;
        PopulateShop();
           if (shopCurrencyText != null)
            shopCurrencyText.text = "Coins: " + currencyManager.playerCurrency.ToString();
    }

    void PopulateShop()
    {
        foreach (Seeds seed in seedItems)
        {
            GameObject button = Instantiate(shopItemButtonPrefab, shopPanel);
            button.transform.Find("Text").GetComponent<Text>().text = seed.itemName;
            button.transform.Find("ValueText").GetComponent<Text>().text = seed.value.ToString();
            button.transform.Find("Image").GetComponent<Image>().sprite = seed.itemIcon;
            Button btnComponent = button.GetComponent<Button>();
            btnComponent.onClick.AddListener(() => OnButtonClick(seed));

            if (currencyManager.playerCurrency < seed.value)
            {
                btnComponent.interactable = false;
            }
        }
        
    }

  void OnButtonClick(Seeds seed)
{
    if (currencyManager.DeductCurrency(seed.value))
    {
        // Check if item already exists in the inventory
        Item existingItem = InventoryManager.Instance.GetItemByID(seed.id);

        if (existingItem != null)
        {
            // Update the quantity of the existing item
            existingItem.quantity += 1;

            Debug.Log($"OnButtonClick: Updated existing item with ID: {existingItem.id}, New Quantity: {existingItem.quantity}");
        }
        else
        {
            // Create a new item
            Item newItem = ScriptableObject.CreateInstance<Item>();
            newItem.id = seed.id;
            newItem.isStackable = seed.isStackable;
            newItem.quantity = 1;
            newItem.itemName = seed.itemName;
            newItem.itemIcon = seed.itemIcon;

            Debug.Log($"OnButtonClick: Created new item with ID: {newItem.id}, Name: {newItem.itemName}, IsStackable: {newItem.isStackable}, Quantity: {newItem.quantity}");

            // Add the new item to the inventory
            InventoryManager.Instance.Add(newItem);
             
             if (newItem.itemName == "Drake Claw Seeds")
            {
                
                objectiveManager.CompleteObjective("buySeed");
            }
        }
    }
    UpdateSeedInventoryDisplay();
    soundManager.PlayObtainedItem();
    
}
 void Update()
    {
        // If the UI is active and the user presses the hotkey, toggle the first time text box
        if (isUIActive && Input.GetKeyDown(KeyCode.K)) // Change KeyCode.K to whatever key you want
        {
            // We only want to toggle if it's not the first time
            if (!isFirstTime)
            {
                isFirstTimeTextBoxActive = !isFirstTimeTextBoxActive;
                firstTimeTextBox.SetActive(isFirstTimeTextBoxActive);
            }
        }
    }

    public void ToggleShopUI()
    {
        isUIActive = !isUIActive;
        shopUI.SetActive(isUIActive);

        if (playerMovement != null)
        {
            playerMovement.isSeedShopOpen = isUIActive;
        }

        if (isUIActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            thirdPersonCam.LockCameraOrientation();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            thirdPersonCam.UnlockCameraOrientation();
            // Also make sure to hide the firstTimeTextBox when closing the UI
            isFirstTimeTextBoxActive = false;
            firstTimeTextBox.SetActive(false);
        }

        if (isFirstTime && isUIActive)
        {
            firstTimeTextBox.SetActive(true);
            isFirstTime = false;
            isFirstTimeTextBoxActive = true; // Make sure to set this true as well
        }
        objectiveManager.CompleteObjective("helloKeeper");
        UpdateButtonState();
        UpdateSeedInventoryDisplay();
        animator.SetTrigger("OnInteract");
    }


    public void UpdateButtonState()
{
    foreach (Transform child in shopPanel)
    {
        string seedNameFromButton = child.Find("Text").GetComponent<Text>().text;
        Seeds seed = seedItems.Find(s => s.itemName == seedNameFromButton);

        if (seed != null)
        {
            bool canPurchase = CanPurchase(seed);
            child.GetComponent<Button>().interactable = canPurchase;
        }
        else
        {
            Debug.LogError("Seed not found: " + seedNameFromButton);
        }
    }

    // Call UpdateShopCurrencyDisplay here instead to ensure it's updated whenever button states are updated
    UpdateShopCurrencyDisplay();
}

    private bool CanPurchase(Seeds seed)
    {
        return currencyManager.playerCurrency >= seed.value;
    }
     private void UpdateShopCurrencyDisplay()
    {
        if (shopCurrencyText != null)
            shopCurrencyText.text = "Coins: " + currencyManager.playerCurrency.ToString();

    }
    public void UpdateSeedInventoryDisplay()
{
    // Clear the current display
    foreach (Transform child in seedInventoryPanel)
    {
        Destroy(child.gameObject);
    }

    // Get the current seeds from the inventory manager
    List<Item> playerSeeds = InventoryManager.Instance.seedItems;

    // Populate the display with the player's seeds
    foreach (Item seed in playerSeeds)
    {
        // Instantiate a new entry prefab and set it as a child of the seedInventoryPanel
        GameObject entry = Instantiate(seedInventoryEntryPrefab, seedInventoryPanel);

        // Set the seed name and quantity
        entry.transform.Find("SeedNameText").GetComponent<Text>().text = seed.itemName;
        entry.transform.Find("SeedQuantityText").GetComponent<Text>().text = seed.quantity.ToString();

        // Set the seed icon
        entry.transform.Find("SeedIconImage").GetComponent<Image>().sprite = seed.itemIcon;
    }
}
}
