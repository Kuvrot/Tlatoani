using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : MonoBehaviour
{

    public float time = 60;
    Image progressImage;
    float clock = 0;

    // Update is called once per frame
    private void Start()
    {
        progressImage = GetComponentInChildren<Image>();
        StartCoroutine(Timer());
    }

    private void Update()
    {
        progressImage.fillAmount = clock/ time;
        clock += 1 * Time.deltaTime;

        if (clock >= time)
        {
            clock = 0;
        }


    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds (time);
        

        if (BuildingManager.instance.currentResources[0] >= 30)
        {
            BuildingManager.instance.AddResource(ResourceType.Food, 20);
            BuildingManager.instance.AddResource(ResourceType.Wood, -30);
            BuildingManager.instance.RefreshResources();
        }

        
        StopAllCoroutines();
        StartCoroutine(Timer());

    }



}
