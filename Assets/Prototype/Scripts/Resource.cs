using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public bool isBigResource = false;
    [SerializeField] ResourceType resourceType; //wood 0 , stone 1, food 2 , gold 3
    [SerializeField] float amount = 0.359f;
    Damageable damageable;
    public bool isHover;

    new

         //HoverVisual
         Renderer renderer;
    private Color emissionColor;

    void Awake()
    {
        damageable = GetComponent<Damageable>();
        damageable.onDestroy.AddListener(GiveResource);
        damageable.onHit.AddListener(HitResource);

        renderer = GetComponent<Renderer>();
        if (renderer)
            emissionColor = renderer.material.GetColor("_EmissionColor");
    }

    void GiveResource()
    {
        BuildingManager.instance.AddResource(resourceType, amount);
    }

    void HitResource()
    {
        //visual
        GiveResource();
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().SetTrigger("shake");
        }
    }

    private void OnMouseEnter()
    {
        isHover = true;
        if (renderer)
            renderer.material.SetColor("_EmissionColor", Color.grey);
    }
    private void OnMouseExit()
    {
        isHover = false;
        if (renderer)
            renderer.material.SetColor("_EmissionColor", emissionColor);
    }

    //Checks if there is a camp close to the resource
    public bool IsCampClose ()
    {

        bool found = false;

        foreach (Building building in BuildingManager.instance.allBuildings)
        {
            if (resourceType.ToString() == "Wood")
            {
                if (building.buildingName == "Lumber camp")
                {
                    float distance = Vector3.Distance(transform.position , building.transform.position);

                    if (distance <= 50)
                    {
                        found =  true;
                    }

                }
            }

            if (resourceType.ToString() == "Stone")
            {

                if (building.buildingName == "Mining camp")
                {
                    float distance = Vector3.Distance(transform.position, building.transform.position);

                    if (distance <= 25)
                    {
                        found = true;
                    }

                }
            }
        }

        Debug.Log(found);
        return found;
     

    }
    

}
