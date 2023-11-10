using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    public int playerCurrency = 0;
    public Text currencyText;
    public Text shopCurrencyText;
    public delegate void CurrencyChangedDelegate();
    public event CurrencyChangedDelegate OnCurrencyChanged;

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

        // Notify subscribers that the currency has changed
        OnCurrencyChanged?.Invoke();

        UpdateCurrencyDisplay();
    }

    public bool DeductCurrency(int amount)
    {
        if (playerCurrency >= amount)
        {
            playerCurrency -= amount;

            // Notify subscribers that the currency has changed
            OnCurrencyChanged?.Invoke();

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
        if (shopCurrencyText != null)
        {
            shopCurrencyText.text = "Coins: " + playerCurrency.ToString();
        }
    }
}
