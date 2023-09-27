using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtPlotInteract : InteractableObject
{
   
    public override void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);

    }
   
}
