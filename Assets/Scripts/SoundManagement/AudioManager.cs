using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public AudioSource ambientSource; // Assign in the inspector
    public AudioClip lowElevationSound; // Assign your low elevation sound
    public AudioClip mediumElevationSound; // Assign your medium elevation sound
    public AudioClip highElevationSound; // Assign your high elevation sound

    public GameObject player; // Assign your player GameObject

    public float mediumElevationThreshold = 50f; // Adjust as needed
    public float highElevationThreshold = 150f; // Adjust as needed

    void Update()
    {
        ChangeAmbientSoundBasedOnElevation();
    }

    void ChangeAmbientSoundBasedOnElevation()
    {
        float playerElevation = player.transform.position.y;

        if (playerElevation < mediumElevationThreshold)
        {
            if (ambientSource.clip != lowElevationSound)
            {
                ambientSource.clip = lowElevationSound;
                ambientSource.Play();
            }
        }
        else if (playerElevation < highElevationThreshold)
        {
            if (ambientSource.clip != mediumElevationSound)
            {
                ambientSource.clip = mediumElevationSound;
                ambientSource.Play();
            }
        }
        else
        {
            if (ambientSource.clip != highElevationSound)
            {
                ambientSource.clip = highElevationSound;
                ambientSource.Play();
            }
        }
    }
}
