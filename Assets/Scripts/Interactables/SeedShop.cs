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
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        currencyManager = CurrencyManager.Instance;
        currencyManager.OnCurrencyChanged += UpdateButtonState;
        PopulateShop();
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
        }
    }
}


    public void ToggleShopUI()
    {
        isUIActive = !isUIActive;
        shopUI.SetActive(isUIActive);

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.isSeedShopOpen = isUIActive;
        }
         if (isUIActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            thirdPersonCam.LockCameraOrientation(); // Add this line
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            thirdPersonCam.UnlockCameraOrientation(); // Add this line
        }
        UpdateButtonState();
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
    }

    private bool CanPurchase(Seeds seed)
    {
        return currencyManager.playerCurrency >= seed.value;
    }
}
