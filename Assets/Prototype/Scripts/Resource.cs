using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] ResourceType resourceType;
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
}
