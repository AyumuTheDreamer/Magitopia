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
                // Check if the nearest object has the "Interactable" tag
                if (nearestObject.CompareTag("Interactable"))
                {
                    // Call the Interact method on the object, which will invoke
                    // the appropriate interaction logic based on its type.
                    InteractableObject interactableObject = nearestObject.GetComponent<InteractableObject>();
                    if (interactableObject != null)
                    {
                        interactableObject.Interact();
                                           

                    }

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
            // Check if the collider's GameObject has the "Interactable" tag
            if (col.CompareTag("Interactable"))
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
}