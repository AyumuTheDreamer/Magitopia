using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public GameObject dialoguePanel;
    private Queue<string> sentences;
    private bool isDialogueActive = false;
    public SoundManager soundManager;

    void Start()
    {
        sentences = new Queue<string>();
    }

    void Update()
    {
        // Check for the 'E' key press when dialogue is active
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
            soundManager.PlayPing();
        }
    }

    public void StartDialogue(string[] dialogueLines)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        sentences.Clear();

        foreach (string sentence in dialogueLines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }
}
