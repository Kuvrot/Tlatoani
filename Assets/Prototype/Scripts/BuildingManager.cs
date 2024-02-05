using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ResourceType { Wood, Stone , Food , Gold }
public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    public List<Building> allBuildings = new List<Building>();
    public Building[] buildingPrefabs = default;
    public float[] currentResources = default;
    public TextMeshProUGUI population;
    public Transform selectedBuilding;

    public int HouseNumber; //This is the quantity of houses in the game, in order to calculate how many villagers can be created


    [SerializeField] private ParticleSystem buildParticle;
   // [SerializeField] private ParticleSystem finishParticle;
    [HideInInspector]
    public BuildingUI ui;
    public GameObject building_ui;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ui = FindObjectOfType<BuildingUI>();
        if (ui)
            ui.RefreshResources();
    }


    private void Update()
    {
        building_ui.SetActive(selectedBuilding != null && selectedBuilding.GetComponent<Building>().IsFinished() 
            && ActorManager.instance.selectedActors.Count == 0 ? true : false);

        population.text = ActorManager.instance.allActors.Count.ToString() + " | " + (HouseNumber * 5 + 4).ToString();

    }

    public void SpawnBuilding(int index, Vector3 position)
    {
        Building building = buildingPrefabs[index];
        if (!building.CanBuild(currentResources))
        {
            ErrorManager.instance.ThrowError(2);
            return;
        }
            

        // Create Building
        building = Instantiate(buildingPrefabs[index], position, Quaternion.identity);
        //allBuildings.Add(building);
        building.ID = allBuildings.Count - 1;
        building.attackable.onDestroy.AddListener(() => RemoveBuilding(building));

        // Give builders build task
        foreach (Actor actor in ActorManager.instance.selectedActors)
        {
            Builder builder = actor as Builder;
            builder.GiveJob(building);
        }

        // Subtract resources
        int[] cost = building.Cost();
        for (int i = 0; i < cost.Length; i++)
        {
            currentResources[i] -= cost[i];
            if (ui)
                ui.RefreshResources();
        }
    }

    public List<Building> GetBuildings()
    {
        return allBuildings;
    }
    public Building GetPrefab(int index)
    {
        return buildingPrefabs[index];
    }

    public Building GetRandomBuilding()
    {
        if (allBuildings.Count > 0)
            return allBuildings[Random.Range(0, allBuildings.Count)];
        else
            return null;
    }
    public void RemoveBuilding(Building building)
    {
        allBuildings.Remove(building);
    }
    public void AddResource(ResourceType resourceType, float amount)
    {
        currentResources[(int)resourceType] += amount;

        if(ui)
            ui.RefreshResources();
    }
    public void PlayParticle(Vector3 position)
    {
        if (buildParticle)
        {
            buildParticle.transform.position = position;
            buildParticle.Play();
        }
    }

}
