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
    void Update()
    {
        // Calculate distance between player and shop
        float distance = Vector3.Distance(playerPosition.position, shopPosition.position);

        // If the distance is greater than maxDistance, disable the prompt. Otherwise, enable it.
        uiUse.gameObject.SetActive(distance <= maxDistance);

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
