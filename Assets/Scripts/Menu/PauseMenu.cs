using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI; // Drag your pause menu UI here in Unity Inspector
    public bool isPaused = false;
    private TimeController timeController; // Reference to your TimeController
    private PlayerMovement playerMovement;
    public ThirdPersonCam thirdPersonCam;
    void Start()
    {
        timeController = FindObjectOfType<TimeController>();
        playerMovement = FindObjectOfType<PlayerMovement>(); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);
        timeController.isPaused = isPaused; // Set isPaused in TimeController
        playerMovement.isGamePaused = isPaused;
        if(isPaused)
        {
            thirdPersonCam.LockCameraOrientation();
        }
        else
        {
            thirdPersonCam.UnlockCameraOrientation();
        }
    }

    public void ContinueGame()
    {
        TogglePause();
        playerMovement.isGamePaused = false;
    }

    public void GoToMainMenu()
    {
        timeController.isPaused = false; // Unpause the time before going back to Main Menu
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }

    public void QuitGame()
    {
        Application.Quit();
        playerMovement.isGamePaused = false;
    }
    void OnEnable()
    {
        // Set as last sibling (will render over all other siblings)
        transform.SetAsLastSibling();
    }
}
