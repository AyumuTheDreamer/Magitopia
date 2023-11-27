using UnityEngine;

public class ThornsGuyDialogue : MonoBehaviour
{
    public string[] dialogueLines;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void TriggerDialogue()
    {
        dialogueManager.StartDialogue(dialogueLines);
    }
}

