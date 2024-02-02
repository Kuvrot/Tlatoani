using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Enemy[] enemyPrefabs = default;

    List<Enemy> allEnemies = new List<Enemy>();
    bool spawn = true;
    private void Update()
    {
        if (spawn && BuildingManager.instance.GetBuildings().Count > 0)
        {
            spawn = false;
            SpawnEnemy(0);
        }
    }
    public void SpawnEnemy(int index)
    {
        Vector3 spawnPosition = Random.insideUnitCircle.normalized * 25;
        spawnPosition.z = spawnPosition.y;
        spawnPosition.y = 0;
        Enemy enemy = Instantiate(enemyPrefabs[index], spawnPosition, Quaternion.identity, transform);
        allEnemies.Add(enemy);
    }
}
