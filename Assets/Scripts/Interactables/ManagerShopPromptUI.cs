using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerShopPromptUI : MonoBehaviour
{
    public Image prefabUI;
    private Image uiUse;
    public Transform shopPosition;
    public Transform playerPosition;  // Reference to the player's Transform
    private Vector3 initialOffset = new Vector3(0, 3f, 0);
    public float maxDistance = 10f;  // Maximum distance for showing the prompt
    public float bobbingSpeed = 0.5f;  // Speed of the bobbing effect
    public float bobbingAmount = 0.5f;  // Height of the bobbing effect
    public float minAngleToShow = 30f;  // Minimum angle to show the prompt
    
    // Start is called before the first frame update
    void Start()
    {
        uiUse = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform).GetComponent<Image>();
        uiUse.transform.SetSiblingIndex(0);

        Canvas secondaryCanvas = uiUse.gameObject.AddComponent<Canvas>();
        secondaryCanvas.overrideSorting = true;
        secondaryCanvas.sortingOrder = -1;
    }

    // Update is called once per frame
    // Update is called once per frame
void Update()
{
    // Calculate distance between player and shop
    float distance = Vector3.Distance(playerPosition.position, shopPosition.position);

    // Calculate the vector from the player to the shop
    Vector3 toShop = (shopPosition.position - playerPosition.position).normalized;

    // Calculate the angle using the camera's forward vector
    float angle = Vector3.Angle(Camera.main.transform.forward, toShop);

    // Check if the player is facing towards the object and within the max distance
    bool shouldShowPrompt = distance <= maxDistance && angle <= minAngleToShow;

    // Enable or disable the prompt based on the conditions
    uiUse.gameObject.SetActive(shouldShowPrompt);

    if (uiUse.gameObject.activeInHierarchy)
    {
        // Calculate the new offset for bobbing effect
        Vector3 offset = initialOffset;
        offset.y += Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;

        // Update the UI position
        uiUse.transform.position = Camera.main.WorldToScreenPoint(shopPosition.position + offset);
    }
}

}