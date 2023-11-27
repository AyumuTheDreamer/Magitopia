using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public Objective[] objectives; // Array of objectives
    public GameObject objectivePanel; // UI Panel for objectives
    public Text objectiveText; // Text component to display objective
    public Image objectiveImage; // Image component for objective

    private int currentObjectiveIndex = 0; // Current objective index
    private bool[] objectiveCompleted; // Tracks if objectives have been completed
    public static ObjectiveManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
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
    // Check if all objectives are completed
    if (currentObjectiveIndex >= objectives.Length)
    {
        // All objectives completed, hide the objective panel
        objectivePanel.SetActive(false);
    }
    else
    {
        // Update the UI for the current objective
        objectiveText.text = objectives[currentObjectiveIndex].description;
        objectiveImage.sprite = objectives[currentObjectiveIndex].image;
    }
}


    public void CompleteObjective(string actionId)
    {
        if (currentObjectiveIndex < objectives.Length && !objectiveCompleted[currentObjectiveIndex])
        {
            if (objectives[currentObjectiveIndex].id == actionId)
            {
                objectiveCompleted[currentObjectiveIndex] = true;
                currentObjectiveIndex++;
                UpdateObjectiveUI();
            }
        }
    }
}
