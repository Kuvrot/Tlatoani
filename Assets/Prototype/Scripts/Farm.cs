using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{

    public float time = 60;

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds (time);
        

        if (BuildingManager.instance.currentResources[0] >= 30)
        {
            BuildingManager.instance.AddResource(ResourceType.Food, 10);
            BuildingManager.instance.AddResource(ResourceType.Wood, -30);
            BuildingManager.instance.RefreshResources();
        }

        
        StopAllCoroutines();
        StartCoroutine(Timer());

    }



}
