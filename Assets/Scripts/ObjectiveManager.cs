using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public Objective[] objectives; // Array of objectives
    public GameObject objectivePanel; // UI Panel for objectives
    public Text objectiveText; // Text component to display objective
    public Image objectiveImage; // Image component for objective

    private int currentObjectiveIndex = 0; // Current objective index
    [SerializeField] private bool[] objectiveCompleted; // Tracks if objectives have been completed

    void Start()
    {
        if (objectives.Length > 0)
        {
            objectiveCompleted = new bool[objectives.Length];
            UpdateObjectiveUI();
        }
        else
        {
            objectivePanel.SetActive(false);
        }
    }

    void UpdateObjectiveUI()
    {
        if (currentObjectiveIndex < objectives.Length)
        {
            objectiveText.text = objectives[currentObjectiveIndex].description;
            objectiveImage.sprite = objectives[currentObjectiveIndex].image;
        }
        else
        {
            objectivePanel.SetActive(false); // Hide panel when all objectives are completed
        }
    }

    public void CompleteObjective()
    {
        if (currentObjectiveIndex < objectives.Length && !objectiveCompleted[currentObjectiveIndex])
        {
            objectiveCompleted[currentObjectiveIndex] = true;
            currentObjectiveIndex++;
            UpdateObjectiveUI();
        }
    }
}
