using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource effectSource; // Assign this in the inspector
    public AudioClip plantingSound;  // Assign your planting sound here
    public AudioClip harvestingSound; // Assign your harvesting sound here
    public AudioClip dragonRoar;
    public AudioClip obtainedItem;
    public AudioClip brewPotion;
    public AudioClip buySeed;
    public static SoundManager Instance { get; private set; }
      void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: if you want it to persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayPlantingSound()
    {
        effectSource.PlayOneShot(plantingSound);
    }

    // Call this method when harvesting
    public void PlayHarvestingSound()
    {
        effectSource.PlayOneShot(harvestingSound);
    }
    public void PlayDragonRoar()
    {
        effectSource.PlayOneShot(dragonRoar);
    }
    public void PlayObtainedItem()
    {
        effectSource.PlayOneShot(obtainedItem);
    }
    public void PlayBrewPotion()
    {
        effectSource.PlayOneShot(brewPotion);
    }
    public void PlayBuySeed()
    {
        effectSource.PlayOneShot(buySeed);
    }
}

