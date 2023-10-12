using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuButtons : MonoBehaviour
{
 
    public void OnStartButtonClick()
    {
        
        SceneManager.LoadScene (1);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
