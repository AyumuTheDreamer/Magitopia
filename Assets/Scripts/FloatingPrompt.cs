using UnityEngine;
using System.Collections.Generic;
public class FloatingPrompt : MonoBehaviour
{
    public GameObject promptCanvas;
    public float visibilityDistance = 20f; // Distance within which the prompt is visible
    public float bobbingSpeed = 1f; // Speed at which the prompt bobs up and down
    public float bobbingAmount = 0.3f; 
    private float originalYPosition;
    private Transform playerTransform;
    private Renderer objectRenderer;
    private bool hasInteracted = false;
    void Start()
    {
        // Assuming the player has a tag "Player"
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // Get the Renderer component of the object you want the prompt to float above
        objectRenderer = GetComponent<Renderer>();
        originalYPosition = promptCanvas.transform.position.y;
    }

    void Update()
    {
        if (hasInteracted) return;
        // Check the distance between the player and this object
        float distance = Vector3.Distance(playerTransform.position, transform.position);
        if (distance <= visibilityDistance)
        {
            // Check if the object is within the camera's view
            if (objectRenderer.isVisible)
            {
                // Enable the prompt canvas
                promptCanvas.SetActive(true);
                RotateTowardsPlayer();
                BobbingEffect();
            }
            else
            {
                // Disable the prompt canvas
                promptCanvas.SetActive(false);
            }
        }
        else
        {
            // Disable the prompt canvas
            promptCanvas.SetActive(false);
        }
    }
      private void RotateTowardsPlayer()
{
    // Determine the direction to the player
    Vector3 directionToPlayer = playerTransform.position - promptCanvas.transform.position;
    // Ensure that the prompt only rotates on the Y axis
    directionToPlayer.y = 0;
    // Set the prompt's rotation to face the direction to the player
    promptCanvas.transform.rotation = Quaternion.LookRotation(directionToPlayer);
}
private void BobbingEffect()
    {
        // Calculate the new Y position using a sine wave
        float newY = originalYPosition + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
        // Set the position of the prompt
        promptCanvas.transform.position = new Vector3(promptCanvas.transform.position.x, newY, promptCanvas.transform.position.z);
    }
     public void DisablePrompt()
{
    promptCanvas.SetActive(false);
    hasInteracted = true;
}

}
