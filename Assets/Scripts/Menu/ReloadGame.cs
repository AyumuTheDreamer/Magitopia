using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadGame : MonoBehaviour
{
    public void LoadGameScene()
    {
        // Replace "GameScene" with the name of your game scene
        SceneManager.LoadScene("SampleScene");
    }
}
