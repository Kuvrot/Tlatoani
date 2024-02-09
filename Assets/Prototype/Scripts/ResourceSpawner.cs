using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceSpawner : MonoBehaviour
{
    public Vector3 initialPosition;
    public int gridSize = 25; // Grid size for spawning resources
    public GameObject[] resourcePrefs;
    public int resourceSpawnChance = 25; // The chance a resource will be spawned

    private List<Vector3> spawnPositions = new List<Vector3>();

    private void Awake()
    {
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
                int ran = Random.Range(0, resourcePrefs.Length);

                Instantiate(resourcePrefs[ran], position, resourcePrefs[ran].transform.rotation);
            }
        }
    }
}
