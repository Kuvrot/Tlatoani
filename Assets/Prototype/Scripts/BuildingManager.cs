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


    [Header("UI")]
    [HideInInspector]
    public BuildingUI ui;
    public GameObject building_ui;
    public Transform resourceGroup;
    public TextMeshProUGUI waveText;


    [Header("SFX")]
    [HideInInspector] public AudioSource audioSource;
    public AudioClip selectBuildingSound , buyingBuildingSound, buildingHitSound;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ui = FindObjectOfType<BuildingUI>();
        if (ui)
            ui.RefreshResources();

        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        building_ui.SetActive(selectedBuilding != null && selectedBuilding.GetComponent<Building>().IsFinished() 
            && ActorManager.instance.selectedActors.Count == 0 ? true : false);

        int maxNumber = HouseNumber * 5 + 8;

        if (HouseNumber >= 68)
        {
            HouseNumber = 68;
        }

        population.text = ActorManager.instance.allActors.Count.ToString() + " | " + maxNumber.ToString();

        waveText.text = "Wave: " + EnemyManager.instance.userWave.ToString();


        //Deleting buildings
        if (selectedBuilding != null)
        {
            // If the building is not finished yet, then the player can get their resources back
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Building building = selectedBuilding.GetComponent<Building>();

                if (!building.IsFinished())
                {
                    for (int i = 0; i < currentResources.Length; i++)
                    {
                        currentResources[i] += building.resourceCost[i];
                        RefreshResources();
                    }
                }
                else
                {
                    allBuildings.Remove(building);
                }

                CursorManager.instance.setBasicCursor();
                audioSource.PlayOneShot(buyingBuildingSound);
                Destroy(selectedBuilding.gameObject);
                selectedBuilding = null;

            }
        }

    }

    public void RefreshResources()
    {
        for (int i = 0; i < resourceGroup.childCount; i++)
            resourceGroup.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = BuildingManager.instance.currentResources[i].ToString("F0");
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
            actor.StopTask();
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
