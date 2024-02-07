using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceSpawner : MonoBehaviour
{
    public Vector3 initialPosition;
    public float increment = 25; //How many spaces the resource spawner will jump
    public GameObject[] resourcePrefs;
    public int resourceSpawnChance = 25; // The chance a resource will be spawned

    float xCounter = 0;
    float yCounter = 0;

    public bool allResources = false;

    bool stoneAdded, goldAdded, woodAdded; // to ensure there is at least all the resources

    void Awake()
    {
        initialPosition = new Vector3 (-315 , 0 ,-476 + increment);
        xCounter = initialPosition.x;
        yCounter = initialPosition.z;
        transform.position = initialPosition;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += new Vector3(increment, 0, 0);
        xCounter += increment;

        int ran = Random.Range(0, 100);

        if (ran <= resourceSpawnChance)
        {
            int ran2 = Random.Range(0, resourcePrefs.Length);

           GameObject prefab = Instantiate(resourcePrefs[ran2], transform.position, transform.rotation);
            
           if (!stoneAdded)
            {
                if (prefab.GetComponent<Resource>().resourceType == ResourceType.Stone)
                {
                    stoneAdded = true;
                }
            }

            if (!woodAdded)
            {
                if (prefab.GetComponent<Resource>().resourceType == ResourceType.Wood)
                {
                    woodAdded = true;
                }
            }

            if (!goldAdded)
            {
                if (prefab.GetComponent<Resource>().resourceType == ResourceType.Gold)
                {
                    goldAdded = true;
                }
            }


        }

        if (stoneAdded && woodAdded && goldAdded)
        {
            allResources = true;
        }

        //Spawn resource
        if (xCounter >= 682)
        {
            yCounter += increment;
            transform.position = new Vector3(initialPosition.x, 0, yCounter);
            xCounter = initialPosition.x;
        }

        if (yCounter >= 525)
        {
            if (allResources)
            { 
                Destroy(gameObject);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
            }
        }

    }
}
