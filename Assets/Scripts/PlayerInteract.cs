using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
   
    public float interactionDistance = 2f;
    private InteractableObject[] interactableObjects;

    private void Update()
    {
        // Check for nearby objects
        interactableObjects = GetInteractableObjects();

        // Check for player input to interact with objects
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Perform interaction with the nearest object
            InteractableObject nearestObject = GetNearestObject();
            if (nearestObject != null)
            {
                nearestObject.Interact();
            }
        }
    }

    private InteractableObject[] GetInteractableObjects()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionDistance);
        InteractableObject[] interactableObjects = new InteractableObject[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            interactableObjects[i] = colliders[i].GetComponentInChildren<InteractableObject>();
        }

        return interactableObjects;
    }

    private InteractableObject GetNearestObject()
    {
        InteractableObject nearestObject = null;
        float nearestDistance = float.MaxValue;

        foreach (InteractableObject obj in interactableObjects)
        {
            if (obj != null)  // Add a null check
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

}
