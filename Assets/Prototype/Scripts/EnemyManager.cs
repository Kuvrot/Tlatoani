using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy[] enemyPrefabs = default;
    static public EnemyManager instance;
    public int wave = 0;
    public float spawnEnemies = 0; //This is the quantity of enemies that must be spawned with the following formula: f(x)=sin(((π)/(100)) x) * 50

    public List<Enemy> allEnemies = new List<Enemy>();
    bool spawn = true;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (BuildingManager.instance.allBuildings.Count > 0)
        {
            if (allEnemies.Count == 0)
            {

                SpawnEnemy();

            }
        }
    }

    public void SpawnEnemy()
    {
        wave += 10;
        spawnEnemies = Mathf.Sin((Mathf.PI / 100) * wave) * 50;

        for (int i = 0; i <= spawnEnemies; i++)
        {
            int ran = Random.Range(0, PortalSpawner.instance.portals.Count);
            GameObject enemy = Instantiate(enemyPrefabs[0].gameObject, PortalSpawner.instance.portals[ran].transform.position, PortalSpawner.instance.portals[ran].transform.rotation);
            allEnemies.Add(enemy.GetComponent<Enemy>());

        }
    }

}
