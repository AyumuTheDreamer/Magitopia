using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
   public Item Item;

   private void OnTriggerEnter(Collider other)
   {
    if(other.CompareTag("Player"))
    {
        Pickup();
    }
   }

   void Pickup()
   {
    InventoryManager.Instance.Add(Item);
    Destroy(gameObject);

   }

}
