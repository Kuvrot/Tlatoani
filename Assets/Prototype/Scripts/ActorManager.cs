using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class ActorManager : MonoBehaviour
{
    public static ActorManager instance;
    public BuildingManager buildingManager;
    [SerializeField] LayerMask actorLayer = default;
    [SerializeField] Transform selectionArea = default;
    public List<Actor> allActors = new List<Actor>();
    public List<Actor> selectedActors = new List<Actor>();
    public List<Actor> selectedSoldiers = new List<Actor>();
    Camera mainCamera;
    Vector3 startDrag;
    Vector3 endDrag;
    Vector3 dragCenter;
    Vector3 dragSize;
    bool dragging;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        mainCamera = Camera.main;

       foreach (Actor actor in GetComponentsInChildren<Actor>()) { 
        
            allActors.Add(actor);
       }

        selectionArea.gameObject.SetActive(false);
    }

    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            dragging = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            startDrag = Utility.MouseToTerrainPosition();
            endDrag = startDrag;

        }
        else if (Input.GetMouseButton(0))
        {
            endDrag = Utility.MouseToTerrainPosition();

            if (Vector3.Distance(startDrag, endDrag) > 1)
            {
                selectionArea.gameObject.SetActive(true);
                dragging = true;
                dragCenter = (startDrag + endDrag) / 2;
                dragSize = (endDrag - startDrag);
                selectionArea.transform.position = dragCenter;
                selectionArea.transform.localScale = dragSize + Vector3.up;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (dragging)
            {
                SelectActors();
                dragging = false;
                selectionArea.gameObject.SetActive(false);

            }
        }

        if (selectedActors.Count > 0)
        {
            buildingManager.ui.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(1))
            {
                if (!dragging)
                {
                    SetTask();
                }
            }

            BuildingManager.instance.selectedBuilding = null;
        }
        else
        {
            
            buildingManager.ui.gameObject.SetActive(false);
        }

    }

    void SetTask()
    {
        if (selectedActors.Count == 0)
            return;
        Collider collider = Utility.CameraRay().collider;
        if (collider.CompareTag("Terrain"))
        {
            if (buildingManager.selectedBuilding == null)
            {
                foreach (Actor actor in selectedActors)
                {
                    actor.StopTask();
                    actor.SetDestination(Utility.MouseToTerrainPosition());
                }
            }
        }
        else if (!collider.CompareTag("Player"))
        { 

            if (collider.TryGetComponent(out Damageable damageable))
            {
                foreach (Actor actor in selectedActors)
                {
                    Builder builder = actor as Builder;
                    builder.StopTask();
                    //use this condition to attack enemy buildings if isBuilder == false
                    if (collider.CompareTag("Building"))
                    {
                        if (!actor.isBuilder)
                        {
                            actor.AttackTarget(damageable);
                        }
                    }
                    else
                    {
                        actor.AttackTarget(damageable);
                    }

                    
                }
            }
        }


    }

    void SelectActors()
    {
        DeselectActors();
        dragSize.Set(Mathf.Abs(dragSize.x / 2), 1, Mathf.Abs(dragSize.z / 2));
        RaycastHit[] hits = Physics.BoxCastAll(dragCenter, dragSize, Vector3.up, Quaternion.identity, 0, actorLayer.value);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out Actor actor))
            {
                if (actor.isBuilder)
                {
                    selectedActors.Add(actor);

                }
                else
                {
                    selectedSoldiers.Add(actor);

                }
                actor.visualHandler.Select();
            }
        }
    }
    public void DeselectActors()
    {
        foreach (Actor actor in selectedActors)
            actor.visualHandler.Deselect();

        selectedActors.Clear();
    }

    private void OnDrawGizmos()
    {
        Vector3 center = (startDrag + endDrag) / 2;
        Vector3 size = (endDrag - startDrag);
        size.y = 1;
        Gizmos.DrawWireCube(center, size);
    }
}
