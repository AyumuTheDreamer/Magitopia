using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public virtual void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }
}