using UnityEngine;

public class CropInteraction : MonoBehaviour
{
    public HarvestableCrop crop; // Reference to the specific crop type 
    public Animator animator;

    public void Harvest()
    {
        animator.SetTrigger("Interact");
        Debug.Log("Harvesting the crop: " + gameObject.name);
        
    }
}