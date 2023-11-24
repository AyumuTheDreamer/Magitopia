using UnityEngine;

public class BookshelfInteract : MonoBehaviour
{
    public GameObject bookListPanel; // Assign in Inspector
    public GameObject[] guidePanels; // Assign in Inspector
    public PlayerMovement playerMovement; // Assign in Inspector

    public void OpenBookList()
    {
        bookListPanel.SetActive(true);
        ControlPlayer(false);
    }

    public void CloseGuide()
    {
        foreach (var panel in guidePanels)
        {
            panel.SetActive(false);
        }
        bookListPanel.SetActive(true);
        ControlPlayer(true);
    }
     public void OpenGuide(int guideIndex)
    {
        // First, close all guide panels
        foreach (var panel in guidePanels)
        {
            panel.SetActive(false);
        }

        // Then, open the selected guide panel
        if (guideIndex >= 0 && guideIndex < guidePanels.Length)
        {
            guidePanels[guideIndex].SetActive(true);
        }
    }

    private void ControlPlayer(bool canMove)
    {
        if (playerMovement != null)
        {
            playerMovement.SetPlayerMovement(canMove);
        }

        Cursor.lockState = canMove ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !canMove;
    }
}
