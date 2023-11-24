using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public AudioSource musicSource; // Assign in the inspector for music
    public AudioSource ambientSource; // Assign in the inspector for ambience

    public AudioClip lowElevationMusic; // Assign your low elevation music
    public AudioClip mediumElevationMusic; // Assign your medium elevation music
    public AudioClip highElevationMusic; // Assign your high elevation music

    public AudioClip lowElevationAmbience; // Assign your low elevation ambience sound
    public AudioClip mediumElevationAmbience; // Assign your medium elevation ambience sound
    public AudioClip highElevationAmbience; // Assign your high elevation ambience sound

    public GameObject player; // Assign your player GameObject

    public float mediumElevationThreshold = 50f; // Adjust as needed
    public float highElevationThreshold = 150f; // Adjust as needed

    void Update()
    {
        float playerElevation = player.transform.position.y;

        PlayMusicBasedOnElevation(playerElevation);
        PlayAmbientSoundBasedOnElevation(playerElevation);
    }

    void PlayMusicBasedOnElevation(float elevation)
    {
        if (elevation < mediumElevationThreshold)
        {
            PlayIfNotPlaying(musicSource, lowElevationMusic);
        }
        else if (elevation < highElevationThreshold)
        {
            PlayIfNotPlaying(musicSource, mediumElevationMusic);
        }
        else
        {
            PlayIfNotPlaying(musicSource, highElevationMusic);
        }
    }

    void PlayAmbientSoundBasedOnElevation(float elevation)
    {
        if (elevation < mediumElevationThreshold)
        {
            PlayIfNotPlaying(ambientSource, lowElevationAmbience);
        }
        else if (elevation < highElevationThreshold)
        {
            PlayIfNotPlaying(ambientSource, mediumElevationAmbience);
        }
        else
        {
            PlayIfNotPlaying(ambientSource, highElevationAmbience);
        }
    }

    void PlayIfNotPlaying(AudioSource audioSource, AudioClip clip)
    {
        if (!audioSource.isPlaying || audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
