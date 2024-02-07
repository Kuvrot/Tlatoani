using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDefense : MonoBehaviour
{
    Building building;
    Transform target;
    public GameObject arrowPref;

    //refreshRate means what would be the refresh rate in order to search an enemy.
    //This implementation searches to avoid refreshing and searching every frame, because it's not necessary.
    public float refreshRate = 4; // in seconds.

    // Start is called before the first frame update
    void Start()
    {
        building = GetComponent<Building>();
        StartCoroutine(SearchEnemy());
    }

    IEnumerator SearchEnemy ()
    {
        yield return new WaitForSeconds(refreshRate);

        if (target == null)
        {
            foreach (Enemy enemy in EnemyManager.instance.allEnemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance <= 50)
                {
                    target = enemy.transform;
                    SpawnArrow();
                }
            }
        }
        else
        {

            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > 50)
            {
                target = null;
            }

            SpawnArrow();
        }

        StopCoroutine(SearchEnemy());
        StartCoroutine(SearchEnemy());


    }

    void SpawnArrow ()
    {

        GameObject arrow = Instantiate(arrowPref, transform.position + new Vector3(0, building.height, 0), transform.rotation);
        arrow.GetComponent<Arrow>().fromBuilding = true;
        arrow.GetComponent<Arrow>().SetTarget(target);

    }

}
