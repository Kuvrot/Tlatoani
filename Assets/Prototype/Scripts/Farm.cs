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
        BuildingManager.instance.AddResource(ResourceType.Food , 20);
        BuildingManager.instance.AddResource(ResourceType.Wood , -60);
        StopAllCoroutines();
        StartCoroutine(Timer());

    }



}
