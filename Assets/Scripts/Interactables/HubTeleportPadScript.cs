using UnityEngine;

public class HubTeleportPadScript : MonoBehaviour
{
    public Transform[] destinations; // Array of teleport destinations
    public string[] destinationNames;
    public Material[] destinationMaterials; // Materials for each destination
    public ParticleSystem destinationChangeEffect; // Particle effect for changing destination
    public ParticleSystem teleportEffect; // Particle effect for teleporting
    private Renderer padRenderer; // Renderer of the teleport pad
    private int currentTargetIndex = 0; // Current target index
    public TeleportPadScript[] singleDestinationPads; // References to single destination pads
    public SoundManager soundManager;

    void Start()
    {
        padRenderer = GetComponent<Renderer>();
    }

    public string GetCurrentDestinationName()
    {
        if (currentTargetIndex >= 0 && currentTargetIndex < destinationNames.Length)
        {
            return destinationNames[currentTargetIndex];
        }
        return "Unknown";
    }

    public void ChangeDestination()
    {
        currentTargetIndex = (currentTargetIndex + 1) % destinations.Length;
        soundManager.PlayPing();
        if (padRenderer != null && destinationMaterials.Length > currentTargetIndex)
        {
            padRenderer.material = destinationMaterials[currentTargetIndex];
        }
        if (destinationChangeEffect != null)
        {
            destinationChangeEffect.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        if (IsDestinationUnlocked(currentTargetIndex))
        {
            Transform playerGroup = other.transform.parent;
            if (playerGroup != null)
            {
                // Move the entire player group
                playerGroup.position = destinations[currentTargetIndex].position;
            }
            else
            {
                // Move only the player model
                other.transform.position = destinations[currentTargetIndex].position;
            }

            // Play the teleport effect
            if (teleportEffect != null)
            {
                teleportEffect.Play();
            }
            soundManager.PlayTeleportSound();
        }
    }
}


    private bool IsDestinationUnlocked(int index)
{
    if (index >= 0 && index < singleDestinationPads.Length)
    {
        return singleDestinationPads[index].IsUnlocked(); // Use IsUnlocked here
    }
    return false;
}

}
