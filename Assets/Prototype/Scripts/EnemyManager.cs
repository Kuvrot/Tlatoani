using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy[] enemyPrefabs = default;
    static public EnemyManager instance;
    public int wave = 0;
    public float spawnEnemies = 0; //This is the quantity of enemies that must be spawned with the following formula: f(x)=sin(((π)/(100)) x) * 50
    public int userWave = 0;

    public List<GameObject> allEnemies;

    public bool playerWon = true;

    private void Awake()
    {
        instance = this;
        wave = -5;
    }

    private void Update()
    {
        if (BuildingManager.instance.allBuildings.Count > 2)
        {
            if (allEnemies.Count == 0)
            {

                SpawnEnemy();

            }
        }

        if (userWave > 12)
        {
            playerWon = true;
            SceneManager.LoadScene(3);
        }

    }

    public void SpawnEnemy()
    {
        wave += 5;
        userWave++;
        spawnEnemies = Mathf.Sin((Mathf.PI / 100) * wave) * 50;

        if (spawnEnemies <= 0)
        {
            spawnEnemies = 5;

        }
        if ( wave > 50)
        {
            spawnEnemies = 100;
        }

        int se = Mathf.RoundToInt(spawnEnemies);

        for (int i = 0; i <= se; i++)
        {
            int ran = Random.Range(0, PortalSpawner.instance.portals.Count);
            GameObject enemy = Instantiate(enemyPrefabs[0].gameObject, PortalSpawner.instance.portals[ran].transform.position, PortalSpawner.instance.portals[ran].transform.rotation);
            allEnemies.Add(enemy.gameObject);

        }
    }

}
