using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    public Transform teleportTarget; // Drag and drop the empty GameObject where you want the player to teleport to
    public ParticleSystem teleportEffect; // Drag and drop the particle system for the teleport effect
    public KeyCode teleportKey = KeyCode.T;
    public float teleportDuration = 5f;

    private bool isTeleporting = false;
    private float teleportTimer = 0f;

    void Update()
    {
        if (Input.GetKey(teleportKey))
        {
            teleportTimer += Time.deltaTime;

            if (!isTeleporting)
            {
                // Enable particle effect when the key is pressed
                teleportEffect.Play();
                isTeleporting = true;
            }

            if (teleportTimer >= teleportDuration)
            {
                // Teleport the player to the target position
                transform.position = teleportTarget.position;

                // Disable particle effect when teleport is complete
                teleportEffect.Stop();
                isTeleporting = false;
                teleportTimer = 0f;
            }
        }
        else
        {
            // Reset the timer and stop the particle effect when the key is released
            teleportTimer = 0f;
            teleportEffect.Stop();
            isTeleporting = false;
        }
    }
}
