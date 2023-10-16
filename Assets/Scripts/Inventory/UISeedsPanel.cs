using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISeedsPanel : MonoBehaviour
{
    public Text seedNameText;
    public Image seedIconImage;
    private List<Seeds> seedList = new List<Seeds>();
    private int selectedSeedIndex = 0;

    private void Start()
    {
        // Initialize the seed list with your available seeds.
        // Populate seedList with your Seeds ScriptableObjects.
        // E.g., seedList = YourFunctionToGetSeedList();

        UpdateUI(); // Update the UI based on the selected seed.
    }

    private void Update()
    {
        // Handle scroll wheel input for changing the selected seed.
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            selectedSeedIndex += (int)Mathf.Sign(scrollInput);
            selectedSeedIndex = Mathf.Clamp(selectedSeedIndex, 0, seedList.Count - 1);

            UpdateUI(); // Update the UI based on the selected seed.
        }
    }

    private void UpdateUI()
    {
        // Display the selected seed's information in the UI.
        if (seedList.Count > 0)
        {
            seedNameText.text = seedList[selectedSeedIndex].seedName;
            seedIconImage.sprite = seedList[selectedSeedIndex].itemIcon;
        }
        else
        {
            seedNameText.text = "";
            seedIconImage.sprite = null;
        }
    }
}
