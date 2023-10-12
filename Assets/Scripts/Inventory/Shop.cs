using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Item itemToSell;

    public void BuyItem()
    {
        InventoryManager.Instance.BuyItem(itemToSell);
    }

    public void SellItem()
    {
        InventoryManager.Instance.SellItem(itemToSell);
    }
}
