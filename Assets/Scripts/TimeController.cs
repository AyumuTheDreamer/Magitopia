using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeController : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier;
    [SerializeField]
    private float startHour;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Light sunLight;
    
    [SerializeField]
    private float sunriseHour;
    [SerializeField]
    private float sunsetHour;
    [SerializeField]
    private Color dayAmbientLight;
    [SerializeField]
    private Color nightAmbientLight;
    [SerializeField]
    private AnimationCurve lightChangeCurve;
    [SerializeField]
    private float maxSunLightIntensity;
    [SerializeField]
    private Light moonLight;
    [SerializeField]
    private float maxMoonLightIntensity;
    private DateTime currentTime;
    public TimeSpan sunriseTime;
    public TimeSpan sunsetTime;
    public int dayCounter = 0;
    public bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    // Update is called once per frame
   void Update()
    {
        if (!isPaused)  // <-- Add this condition
        {
            UpdateTimeOfDay();
            RotateSun();
            UpdateLightSettings();
            
            // Check for midnight (00:00)
            if (currentTime.TimeOfDay.Hours == 0 && currentTime.TimeOfDay.Minutes == 0)
            {
                dayCounter++;
                IncrementCropGrowthForAll();
            }
        }
    }

   private void UpdateTimeOfDay()
{
    currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

    if (timeText != null)
    {
        // Use "hh:mm tt" to get the 12-hour format with AM/PM
        timeText.text = currentTime.ToString("hh:mm tt");
    }
}

    private void RotateSun()
    {
        float sunLightRotation;

        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));

        // Line to adjust the skybox color
        Color skyboxTintColor = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.skybox.SetColor("_Tint", skyboxTintColor);
        
        // Line to adjust the ambient light color
        Color ambientLightColor = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = ambientLightColor;
    }


    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
    public void SetCurrentTime(int hour, int minute)
    {
    currentTime = DateTime.Now.Date + new TimeSpan(hour, minute, 0);
    }
    
    public int GetDayCounter()
    {
        return dayCounter;
    }
  public DateTime GetCurrentTime()
{
    return currentTime;
}
 public void IncrementCropGrowthForAll()
    {
        CropInteraction[] cropInteractions = FindObjectsOfType<CropInteraction>();
        foreach (CropInteraction crop in cropInteractions)
        {
            crop.IncrementGrowthByDay();
        }
    }

}
