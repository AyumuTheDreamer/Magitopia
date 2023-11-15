using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText; // Assign in inspector, the UI Text element to display dialogue
    public string[] dialogueLines; // The lines of dialogue
    public GameObject dialoguePanel; // The panel that contains the dialogue UI
    public ShrineInteraction shrineInteraction;
    private int currentLine = 0;

    void Start()
    {
        dialoguePanel.SetActive(false); // Hide dialogue panel initially
    }

    public void StartDialogue()
{
    FindObjectOfType<UIManager>().ToggleCutsceneUI(true);
    currentLine = 0;
    ShowLine();
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialoguePanel.activeSelf)
        {
            currentLine++;
            if (currentLine < dialogueLines.Length)
            {
                ShowLine();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    void ShowLine()
    {
        dialogueText.text = dialogueLines[currentLine];
    }

    void EndDialogue()
    {
        FindObjectOfType<UIManager>().ToggleCutsceneUI(false);
        if (shrineInteraction != null)
        {
            shrineInteraction.EndCutscene();
        }
    }
}
