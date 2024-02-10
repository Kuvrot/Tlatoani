using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


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
    public bool dragging;

    //Components
    AudioSource audioSource;
    public Transform marker;

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

        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {

        marker.gameObject.SetActive(selectedActors.Count > 0);


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
                //DeselectActors();
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
                    BuildingManager.instance.selectedBuilding = null;
                    audioSource.Play();
                }

            }

        }
        else
        {
            
            buildingManager.ui.gameObject.SetActive(false);
        }

        if (allActors.Count <= 0)
        {
            SceneManager.LoadScene(4);
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
                    if (actor.isArcher)
                    {
                        actor.agent.stoppingDistance = 2;
                    }

                    actor.StopTask();
                    actor.SetDestination(Utility.MouseToTerrainPosition());
                    marker.transform.position = Utility.MouseToTerrainPosition();

                }
            }
        }
        else if (!collider.CompareTag("Player"))
        { 

            if (collider.TryGetComponent(out Damageable damageable))
            {

                bool resource = false;


                if (damageable.GetComponent<Resource>())
                {
                 
                        if (damageable.GetComponent<Resource>().IsCampClose())
                        {
                            resource = true;
                        }

                }

                    foreach (Actor actor in selectedActors)
                {
                    Builder builder = actor as Builder;
                    builder.StopTask();
                    //use this condition to attack enemy buildings if isBuilder == false

                    if (actor.isArcher)
                    {
                        actor.agent.stoppingDistance = 20;
                    }

                    if (damageable.GetComponent<Resource>())
                    {

                        if (damageable.GetComponent<Resource>().isBigResource)
                        {

                            if (resource)
                            {
                                Attack(actor, collider, damageable);
                            }
                            else
                            {
                                ErrorManager.instance.ThrowError(0);
                            }

                        }
                        else
                        {
                            Attack(actor, collider, damageable);
                        }
                            
                    }
                    else
                    {
                        Attack(actor , collider , damageable);
                    }
                    

                    
                }
            }
        }


    }

    void Attack(Actor actor , Collider collider , Damageable damageable)
    {
        if (collider.CompareTag("Building"))
        {
            if (!actor.isBuilder)
            {
                actor.AttackTarget(damageable);
            }
        }
        else
        {

            if (!actor.isBuilder)
            {
                actor.AttackTarget(damageable);
            }
            else
            {
                if (damageable.TryGetComponent(out Resource resource ))
                {
                    actor.AttackTarget(damageable);
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
                selectedActors.Add(actor);
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
