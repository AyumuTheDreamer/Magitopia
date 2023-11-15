using UnityEngine;

public class FirefliesController : MonoBehaviour
{
    private TimeController timeController;
    [SerializeField] public GameObject fireflies;

    void Start()
    {
        // Find the TimeController in the scene
        timeController = FindObjectOfType<TimeController>();

        // Assuming this script is attached to the Fireflies GameObject
        fireflies = this.gameObject;
    }

    void Update()
    {
        UpdateFirefliesActivity();
    }

    private void UpdateFirefliesActivity()
    {
        if (timeController == null) return;

        var currentTime = timeController.GetCurrentTime().TimeOfDay;
        var sunriseTime = timeController.sunriseTime;
        var sunsetTime = timeController.sunsetTime;

        // Activate fireflies between sunset and sunrise
        if (currentTime > sunsetTime || currentTime < sunriseTime)
        {
            fireflies.SetActive(true);
        }
        else
        {
            fireflies.SetActive(false);
        }
    }
}
