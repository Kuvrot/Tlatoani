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

        if (building.buildingIsAdded)
        {
            

            if (target == null)
            {
                if (EnemyManager.instance.allEnemies.Count > 0)
                {
                    foreach (GameObject enemy in EnemyManager.instance.allEnemies)
                    {

                        if (enemy != null)
                        {
                            float distance = Vector3.Distance(transform.position, enemy.transform.position);

                            if (distance <= 50)
                            {
                                target = enemy.transform;
                                SpawnArrow();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {

                if (EnemyManager.instance.allEnemies.Count > 0)
                {
                    float distance = Vector3.Distance(transform.position, target.position);

                    if (distance > 50)
                    {
                        target = null;
                    }
                    else
                    {
                        if (target != null)
                        {
                            SpawnArrow();
                        }
                    }

                }


            }

            
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
