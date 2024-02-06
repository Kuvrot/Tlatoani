using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingMenuManager : MonoBehaviour
{

    //Keep adding...
    public GameObject[] Menus;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in Menus)
        {
            obj.SetActive(false);
        }
    }

    public void BuildingMenu ()
    {
        if (BuildingManager.instance.selectedBuilding != null)
        {
            string names = BuildingManager.instance.selectedBuilding.GetComponent<Building>().buildingName;

            if (names == "Settlement")
            {
                BuildingManager.instance.building_ui = Menus[1];

            }else if (names == "Eagle house")
            {
                BuildingManager.instance.building_ui = Menus[2];

            }else if (names == "Fortress")
            {
                BuildingManager.instance.building_ui = Menus[3];
            }
            else if (names == "Archery")
            {
                BuildingManager.instance.building_ui = Menus[4];
            }
            else
            {
                BuildingManager.instance.building_ui = Menus[0];
            }
        }

        foreach (GameObject obj in Menus)
        {
            obj.SetActive(false);
        }
    }

}
