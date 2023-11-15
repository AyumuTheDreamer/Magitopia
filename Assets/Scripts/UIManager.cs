using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject[] uiElementsToToggle; // Assign all UI elements you want to toggle in the inspector
    public GameObject dialoguePanel; // Assign the dialogue panel

    public void ToggleCutsceneUI(bool showDialogue)
    {
        foreach (var element in uiElementsToToggle)
        {
            element.SetActive(!showDialogue);
        }

        dialoguePanel.SetActive(showDialogue);
    }
}

