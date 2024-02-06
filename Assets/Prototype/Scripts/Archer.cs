using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public Transform arrowPrefab;

    // Update is called once per frame
    public void SpawnArrows(Damageable target)
    {
        GameObject arrow = Instantiate(arrowPrefab.gameObject, transform.position + new Vector3 (0 , 2.2f , 0), transform.rotation);
        arrow.GetComponent<Arrow>().SetTarget(target.transform);
    }
}
