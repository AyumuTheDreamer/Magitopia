using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.SceneManagement;

public class ShrineInteraction : MonoBehaviour
{
    public GameObject firstShrineModel;
    public GameObject secondShrineModel;
    public GameObject dragonModel; // Reference to the dragon model
    public float dragonAppearanceDelay = 2.0f; // Delay in seconds before the dragon appears
    public float cameraSwitchBackDelay = 5.0f; // Delay in seconds before switching back to the main camera
   public GameObject mainCameraGameObject; // Reference to the GameObject with the main camera (Cinemachine FreeLook)
    public GameObject cutsceneCameraGameObject; // Reference to the cutscene Cinemachine camera
    public ParticleSystem vortexEffect; // Particle system for the vortex effect
    public DialogueSystem dialogueSystem;
    public SoundManager soundManager;

    public void Interact()
    {
        // Deactivate the first shrine model and activate the second
        firstShrineModel.SetActive(false);
        secondShrineModel.SetActive(true);

        // Start the vortex particle effect
        if (vortexEffect != null)
        {
            Debug.Log("Playing vortex effect");
            vortexEffect.Play();
        }

        // Start coroutine to activate the dragon model after a delay
        StartCoroutine(ActivateDragon());
        SwitchToCutsceneCamera();
        dialogueSystem.StartDialogue();
    }

    IEnumerator ActivateDragon()
    {
        // Wait for the specified delay, then activate the dragon model
        yield return new WaitForSeconds(dragonAppearanceDelay);
        dragonModel.SetActive(true);
        soundManager.PlayDragonRoar();
        
    }

 void SwitchToCutsceneCamera()
    {
        if (cutsceneCameraGameObject != null) cutsceneCameraGameObject.SetActive(true);
        if (mainCameraGameObject != null) mainCameraGameObject.SetActive(false);
    }

   public void SwitchToMainCamera()
    {
        if (cutsceneCameraGameObject != null) cutsceneCameraGameObject.SetActive(false);
        if (mainCameraGameObject != null) mainCameraGameObject.SetActive(true);
    }
    public void EndCutscene()
    {
        SwitchToMainCamera();
        LoadEndScene();
    }
    public void LoadEndScene()
{
    SceneManager.LoadScene("EndScreen");
}

}
