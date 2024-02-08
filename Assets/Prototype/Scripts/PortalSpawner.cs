using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSpawner : MonoBehaviour
{
    static public PortalSpawner instance;
    public Vector3 initialPosition;
    public int gridSize = 25; // Grid size for spawning resources
    public GameObject[] resourcePrefs;
    public int resourceSpawnChance = 25; // The chance a resource will be spawned
    public List<GameObject> portals;

    private List<Vector3> spawnPositions = new List<Vector3>();

    private void Awake()
    {
        instance = this;
        initialPosition = new Vector3(-315, 0, -476 + gridSize);
        GenerateSpawnPositions();
        SpawnResources();
    }

    private void GenerateSpawnPositions()
    {
        for (float x = initialPosition.x; x < 682; x += gridSize)
        {
            for (float z = initialPosition.z; z < 525; z += gridSize)
            {
                spawnPositions.Add(new Vector3(x, 0, z));
            }
        }
    }

    private void SpawnResources()
    {
        foreach (Vector3 position in spawnPositions)
        {
            if (Random.Range(0, 100) <= resourceSpawnChance)
            {
                GameObject portal = Instantiate(resourcePrefs[Random.Range(0, resourcePrefs.Length)], position, Quaternion.identity);
                portals.Add(portal);

                if (portals.Count > 10)
                {
                    break;
                }

            }
        }
    }
}
