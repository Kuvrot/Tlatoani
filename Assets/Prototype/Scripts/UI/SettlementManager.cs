using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettlementManager : MonoBehaviour
{

    //This is the time it will take to create a villager
    public float time = 25;

    public GameObject villagerPrefab;

    public List<Transform> buildingsWorking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Two lists will be made one for the building and one for his progress, and will look like this
        // building | Progress (sec)
        if (buildingsWorking.Count > 0)
        {
            for (int i = 0; i < buildingsWorking.Count; i++)
            {
                
                Building building = buildingsWorking[i].GetComponent<Building>();
                Image progress = buildingsWorking[i].GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
                progress.fillAmount = ((building.clock * 100) / time) * 0.01f;
                building.clock += 1 * Time.deltaTime;

                //Create villager
                if (building.clock >= time)
                {
                    progress.fillAmount = 0;
                    building.queue--;
                    //Spawn a villager int the entrance of the building
                    GameObject villager = Instantiate(villagerPrefab, buildingsWorking[i].position + new Vector3 (8,0,-8), buildingsWorking[i].rotation);
                    villager.transform.parent = ActorManager.instance.transform;
                    //Add the villager to the allActors list
                    ActorManager.instance.allActors.Add(villager.GetComponent<Actor>());
                    building.clock = 0;
                }

                if (building.queue <= 0)
                {
                    buildingsWorking.Remove(buildingsWorking[i]);
                }


            }
        }
    }


    public void CreateVillager()
    {

        if (ActorManager.instance.allActors.Count + 1 <= (BuildingManager.instance.HouseNumber * 5 + 4))
        {
            if (checkIfExist())
            {
                BuildingManager.instance.selectedBuilding.GetComponent<Building>().queue++;
            }
            else
            {
                buildingsWorking.Add(BuildingManager.instance.selectedBuilding);
                BuildingManager.instance.selectedBuilding.GetComponent<Building>().queue++;
            }
        }
        else
        {
            ErrorManager.instance.ThrowError(1);
        }

    }


    bool checkIfExist()
    {

        bool found = false;

        for (int i = 0; i < buildingsWorking.Count; i++)
        {
            if (buildingsWorking[i] == BuildingManager.instance.selectedBuilding)
            {
                found = true;
                break;
            }
        }

        return found;
 

    }


}
