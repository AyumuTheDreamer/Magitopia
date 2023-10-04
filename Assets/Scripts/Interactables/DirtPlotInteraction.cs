
using UnityEngine;

public class DirtPlotInteraction : InteractableObject
{
   public Animator animator;
    public void InteractWithDirtPlot()
    {
        Debug.Log("Interacting with " + gameObject.name);
        animator.SetTrigger("Plant");

    }
   
}
