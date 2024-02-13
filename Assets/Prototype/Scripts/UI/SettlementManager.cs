using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettlementManager : MonoBehaviour
{

    //This is the time it will take to create a villager
    public float time = 25;

    public GameObject villagerPrefab;

    public float[] cost = {0 , 0 , 50 , 0};

    public List<Transform> buildingsWorking;

    public AudioClip[] SFX; //0 is when pressing a button, 1 is when a unit is created.

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                    audioSource.PlayOneShot(SFX[1]);
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

        audioSource.PlayOneShot(SFX[0]);

        if (ActorManager.instance.allActors.Count + 1 <= (BuildingManager.instance.HouseNumber * 5 + 8) && ActorManager.instance.allActors.Count < 68)
        {
            if (EnoughResources())
            {

                BuildingManager.instance.AddResource(ResourceType.Wood, -cost[0]);
                BuildingManager.instance.AddResource(ResourceType.Stone, -cost[1]);
                BuildingManager.instance.AddResource(ResourceType.Food, -cost[2]);
                BuildingManager.instance.AddResource(ResourceType.Gold, -cost[3]);

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
                ErrorManager.instance.ThrowError(2);
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


    bool EnoughResources ()
    {
        bool isEnough = true;

        for (int i = 0; i < cost.Length; i++)
        {
            if (cost[i] > BuildingManager.instance.currentResources[i])
            {
                isEnough = false;
                break;
            }
        }
        
        return isEnough;


    }


}
