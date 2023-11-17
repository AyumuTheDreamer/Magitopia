using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer gameAudioMixer;

    public void SetMusicVolume(float volume)
    {
        gameAudioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        gameAudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetAmbientVolume(float volume)
    {
        gameAudioMixer.SetFloat("Ambient", Mathf.Log10(volume) * 20);
    }
}
