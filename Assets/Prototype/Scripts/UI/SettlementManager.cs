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
    public List<float> buildingsWorkingProgress;
    public List<int> queue; 


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
                progress.fillAmount = ((buildingsWorkingProgress[i] * 100) / time) * 0.01f;
                buildingsWorkingProgress[i] += 1 * Time.deltaTime;

                //Create villager
                if (buildingsWorkingProgress[i] >= time)
                {
                    progress.fillAmount = 0;
                    building.queue--;
                    //Spawn a villager int the entrance of the building
                    GameObject villager = Instantiate(villagerPrefab, buildingsWorking[i].position + new Vector3 (8,0,-8), buildingsWorking[i].rotation);
                    villager.transform.parent = ActorManager.instance.transform;
                    //Add the villager to the allActors list
                    ActorManager.instance.allActors.Add(villager.GetComponent<Actor>());
                    buildingsWorkingProgress[i] = 0;
                }

                if (building.queue <= 0)
                {
                    buildingsWorking.Remove(buildingsWorking[i]);
                    buildingsWorkingProgress.Remove(buildingsWorkingProgress[i]);
                }


            }
        }
    }


    public void CreateVillager()
    {
        buildingsWorking.Add(BuildingManager.instance.selectedBuilding);
        buildingsWorkingProgress.Add(0);
        BuildingManager.instance.selectedBuilding.GetComponent<Building>().queue++;


    }


}
