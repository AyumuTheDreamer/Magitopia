using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    public Transform playerTransform; // Drag your player/camera transform here in Unity Editor
    public float highAltitude = 100f; // The elevation level where the fog is thickest
    public float lowAltitude = 0f; // The elevation level where the fog is least thick

    public float highFogDensity = 0.05f; // Fog density at high altitude
    public float lowFogDensity = 0.01f; // Fog density at low altitude

    void Update()
    {
        // Get the player's (or camera's) elevation
        float elevation = playerTransform.position.y;

        // Calculate the interpolated fog density
        float fogDensity = Mathf.Lerp(lowFogDensity, highFogDensity, (elevation - lowAltitude) / (highAltitude - lowAltitude));

        // Update the fog settings. This applies globally.
        RenderSettings.fogDensity = fogDensity;
    }
}
