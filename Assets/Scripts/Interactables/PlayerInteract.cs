using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactionDistance = 2f;
    private GameObject[] interactableObjects;

    public Animator animator;

    private void Update()
    {
        // Check for nearby objects
        interactableObjects = GetInteractableObjects();

        // Check for player input to interact with objects
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Perform interaction with the nearest object
            GameObject nearestObject = GetNearestObject();
            if (nearestObject != null)
            {
                if (nearestObject.CompareTag("Interactable"))
                {
                    // Handle interaction with objects tagged as "Interactable"
                    HandleInteractableInteraction(nearestObject);
                }
                else if (nearestObject.CompareTag("CropForPickup"))
                {
                    // Handle interaction with objects tagged as "CropForPickup"
                    HandleCropInteraction(nearestObject);
                }
                else if (nearestObject.CompareTag("Plantable"))
                {
                    HandleDirtPlotInteraction(nearestObject);
                }
               
            }
        }
    }

    private GameObject[] GetInteractableObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        List<GameObject> interactableObjectList = new List<GameObject>();

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Interactable") || col.CompareTag("CropForPickup") || col.CompareTag("Plantable"))
            {
                interactableObjectList.Add(col.gameObject);
            }
        }

        return interactableObjectList.ToArray();
    }

    private GameObject GetNearestObject()
    {
        GameObject nearestObject = null;
        float nearestDistance = float.MaxValue;

        foreach (GameObject obj in interactableObjects)
        {
            if (obj != null)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestObject = obj;
                    nearestDistance = distance;
                }
            }
        }

        return nearestObject;
    }

    private void HandleInteractableInteraction(GameObject interactableObject)
    {
        Debug.Log("Interacting with an object");
    }

    private void HandleCropInteraction(GameObject cropObject)
    {
         // Check if the cropObject has the CropInteraction script attached
    CropInteraction cropInteraction = cropObject.GetComponent<CropInteraction>();

         // Check if the script component was found
         if (cropInteraction != null)
          {
             // Call the Harvest method from CropInteraction
              cropInteraction.Harvest();
           }
    }
    private void HandleDirtPlotInteraction(GameObject dirtPlotObject)
{
    DirtPlotInteraction dirtPlotInteraction = dirtPlotObject.GetComponent<DirtPlotInteraction>();

    // Check if the script component and tag are found
    if (dirtPlotInteraction != null && dirtPlotObject.CompareTag("Plantable") && dirtPlotInteraction.isPlantable)
    {
        // Call the appropriate method from DirtPlotInteraction
        dirtPlotInteraction.InteractWithDirtPlot();
        Debug.Log("Interacting with " + dirtPlotObject.name);
    }
    else
    {
        // Handle cases where the dirt plot is not plantable or doesn't have the script/tag
        Debug.LogWarning("Dirt plot cannot be planted or is missing components.");
    }
}

    
}
