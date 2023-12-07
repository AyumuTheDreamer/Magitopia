using UnityEngine;

public class TeleportPadScript : MonoBehaviour
{
    public Transform teleportDestination; // Assign the hub pad's location
    private bool isUnlocked = false;
    private float unlockTimer = 0f;
    private const float unlockDuration = 0.5f; // Duration for double tap detection
    public ParticleSystem unlockEffect;
    public SoundManager soundManager;
    void Update()
    {
        if (unlockTimer > 0)
        {
            unlockTimer -= Time.deltaTime;
        }
    }

    public void HandleInteraction()
    {
        if (unlockTimer > 0 && !isUnlocked)
        {
            // Double tap detected, unlock the pad
            isUnlocked = true;
            Debug.Log("Teleport pad unlocked");
            unlockEffect.Play();
        }
        else
        {
            unlockTimer = unlockDuration;
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player") && isUnlocked)
    {
        Transform playerGroup = other.transform.parent;
        if (playerGroup != null)
        {
            // Move the entire player group
            playerGroup.position = teleportDestination.position;
        }
        else
        {
            // Move only the player model
            other.transform.position = teleportDestination.position;
        }
        soundManager.PlayTeleportSound();
    }
}


    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
