using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int playerCurrency = 0;
    public Text currencyText;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        // Find the Text component in the InventoryManager
        currencyText = InventoryManager.Instance.currencyText;

        // Update the currency display
        UpdateCurrencyDisplay();
    }
    public void AddCurrency(int amount)
    {
        playerCurrency += amount;
        UpdateCurrencyDisplay();
    }

    public bool DeductCurrency(int amount)
    {
        if (playerCurrency >= amount)
        {
            playerCurrency -= amount;
            UpdateCurrencyDisplay();
            return true;
        }
        return false; // Not enough currency to deduct.
    }

    private void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
        {
            currencyText.text = "Coins: " + playerCurrency.ToString();
        }
    }
}