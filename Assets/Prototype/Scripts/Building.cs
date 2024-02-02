using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Damageable))]
public class Building : MonoBehaviour
{
    public bool isSelected = false;
    public int ID = 0;
    public string buildingName;
    [SerializeField] float height;
    public float radius = 5;
    float originalHeight;
    [SerializeField] int totalWorkToComplete = 100;
    int currentWork;
    public int[] resourceCost = default;
    Transform buildingTransform;
    [HideInInspector] public Damageable attackable;
    public bool isHover = false;
    private bool done;
    [ColorUsage(true, true)]
    [SerializeField] private Color[] stateColors;
    MeshRenderer buildingRender;
    Cinemachine.CinemachineImpulseSource impulse;

    //UI
    [HideInInspector()]
    public Slider health_bar;

    private void Awake()
    {
        attackable = GetComponent<Damageable>();
    }

    void Start()
    {

        buildingTransform = transform.GetChild(0);
        buildingRender = buildingTransform.GetComponent<MeshRenderer>();
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
        currentWork = 0;
        originalHeight = buildingTransform.localPosition.y;
        buildingTransform.localPosition = Vector3.down * height;
        health_bar = GetComponentInChildren<Slider>();
        health_bar.maxValue = totalWorkToComplete;
    }

    private void Update()
    {
        
        if (isHover)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Select();
                ActorManager.instance.DeselectActors();
            }
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Deselect();
            }
        }
        
        if (isSelected)
        {
            
        }
    }

    void Deselect()
    {
        isSelected = false;
        BuildingManager.instance.selectedBuilding = null;
    }

    void Select()
    {
        isSelected = true;

        if (!IsFinished())
        {
            BuildingManager.instance.selectedBuilding = this.transform;
        }
    }

    public void Build(int work)
    {
        currentWork += work;
        attackable.currentHealth += work;
        health_bar.value = attackable.currentHealth;
        buildingTransform.localPosition = Vector3.Lerp(Vector3.down * height, new Vector3(0,originalHeight,0), (float) currentWork / totalWorkToComplete);

        //visual
        BuildingManager.instance.PlayParticle(transform.position);
    }
    public bool IsFinished()
    {
        if (currentWork >= totalWorkToComplete && !done && buildingRender)
        {
            done = true;
            if (impulse)
                impulse.GenerateImpulse();
        }
        return currentWork >= totalWorkToComplete;
    }
    public bool CanBuild(float[] resources)
    {
        bool canBuild = true;
        for (int i = 0; i < resourceCost.Length; i++)
        {
            if (resources[i] < resourceCost[i])
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;
    }
    public int[] Cost()
    {
        return resourceCost;
    }

    private void OnMouseEnter()
    {
        isHover = true;
    }
    private void OnMouseExit()
    {
        isHover = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
