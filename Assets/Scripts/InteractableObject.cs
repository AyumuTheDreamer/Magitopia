using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);

        Destroy(gameObject);
    }
}
