using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Notification : MonoBehaviour
{
    public Image itemIcon;
    public Text quantityText;
    public float displayDuration = 2.0f;

    private void Start()
    {
        // Start the fade-out coroutine
        StartCoroutine(FadeOutNotification());
    }

    public void SetNotification(Sprite icon, int quantity)
    {
        itemIcon.sprite = icon;
        quantityText.text = quantity.ToString();
    }

    private IEnumerator FadeOutNotification()
    {
        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Start fading out - you might need to adjust this logic depending on your UI setup
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (!canvasGroup)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / displayDuration;
            yield return null;
        }

        Destroy(gameObject); // Destroy the notification after fade out
    }
}
