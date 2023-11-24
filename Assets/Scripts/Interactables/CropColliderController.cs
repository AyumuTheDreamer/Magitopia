using UnityEngine;

public class CropColliderController : MonoBehaviour
{
    private Collider cropCollider;
    public CropInteraction cropInteraction;

    void Start()
    {
        cropCollider = GetComponent<Collider>();
        if (cropCollider == null)
        {
            Debug.LogError("Collider not found on the crop.");
            return;
        }

        if (cropInteraction == null)
        {
            Debug.LogError("CropInteraction script not assigned.");
            return;
        }

        // Initial collider state update
        UpdateColliderState();
    }

    void Update()
    {
        // Update the collider state if needed
        UpdateColliderState();
    }

    private void UpdateColliderState()
    {
        if (cropInteraction.IsFullyGrown())
        {
            cropCollider.enabled = true;
        }
        else
        {
            cropCollider.enabled = false;
        }
    }
}
