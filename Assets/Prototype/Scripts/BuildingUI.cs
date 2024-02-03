using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    bool isPlacing = false;
    int currentIndex = 0;
    public Transform targetRotation;

    public Transform resourceGroup;

    Mesh buildingPreviewMesh;
    [SerializeField] Material buildingPreviewMat;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Start()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[index].onClick.AddListener(() => SelectBuilding(index));

            Building b = BuildingManager.instance.buildingPrefabs[index];
            buttons[index].GetComponentInChildren<TextMeshProUGUI>().text = GetButtonText(b);
        }
    }

    private void Update()
    {
        if (isPlacing)
        {
            Vector3 position = Utility.MouseToTerrainPosition();
            Graphics.DrawMesh(buildingPreviewMesh, position, targetRotation.rotation , buildingPreviewMat, 0);
            if (Input.GetMouseButtonDown(0))
            {
                

                BuildingManager.instance.SpawnBuilding(currentIndex, position);
                canvasGroup.alpha = 1;
                ActorManager.instance.DeselectActors();
                isPlacing = false;
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                isPlacing = false;
                canvasGroup.alpha = 1;
            }
        }
    }

    void SelectBuilding(int index)
    {
        currentIndex = index;
        //ActorManager.instance.DeselectActors();
        canvasGroup.alpha = 0;
        isPlacing = true;
        buildingPreviewMesh = BuildingManager.instance.GetPrefab(index).GetComponentInChildren<MeshFilter>().sharedMesh;
    }

    string GetButtonText(Building b)
    {
        string buildingName = b.buildingName;
        int resourceAmount = b.resourceCost.Length;
        string[] resourceNames = new string[] { "Wood", "Stone" , "Gold" , "Food"};
        string resourceString = string.Empty;
        for (int j = 0; j < resourceAmount; j++)
            resourceString += "\n " + resourceNames[j] + " (" + b.resourceCost[j] + ")";

        return "<size=11><b>" + buildingName + "</b></size>" + resourceString;
    }

    public void RefreshResources()
    {
        for (int i = 0; i < resourceGroup.childCount; i++)
            resourceGroup.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = BuildingManager.instance.currentResources[i].ToString("F0");
    }
}
