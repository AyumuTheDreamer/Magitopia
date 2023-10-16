using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    public Item item;

    public Button RemoveButton;

    public void RemoveItem()
{
    if (item != null)
    {
        Debug.Log("Removing item: " + item.name);
        InventoryManager.Instance.Remove(item);
        Debug.Log("RemoveItem method called");
        Destroy(gameObject);
    }
    else
    {
        Debug.LogError("Item is null");
    }
}
}
