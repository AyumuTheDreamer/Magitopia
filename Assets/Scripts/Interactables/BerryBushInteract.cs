using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBushInteract : InteractableObject
{
   public Animator animator;

   public int berryCountToAdd = 5;
    public override void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
        animator.SetTrigger("Interact");
        
    }
   
}