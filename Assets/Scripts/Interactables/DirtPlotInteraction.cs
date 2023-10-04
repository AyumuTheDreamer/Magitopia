using UnityEngine;

public class DirtPlotInteraction : MonoBehaviour
{
    public Animator animator;
    public bool isPlantable = true; // Flag to check if the dirt plot is plantable

    // Function to handle the interaction with the dirt plot
    public void InteractWithDirtPlot()
    {
        if (isPlantable)
        {
            Debug.Log("Interacting with " + gameObject.name);
            // Trigger the "Plant" animation
            animator.SetTrigger("Plant");
            // You can add any additional logic here, such as planting seeds or other actions.
        }
        else
        {
            Debug.Log("Dirt plot is not plantable.");
            // Optionally, you can add a message or sound effect indicating that the plot is not ready for planting.
        }
    }
}