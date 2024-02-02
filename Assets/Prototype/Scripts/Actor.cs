using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Damageable))]
public class Actor : MonoBehaviour
{
    NavMeshAgent agent;
    public bool isBuilder = true;
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Damageable damageableTarget;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimationEventListener animationEvent;
    public Coroutine currentTask;
    [HideInInspector] public ActorVisualHandler visualHandler;

    public bool isHover = false;
    bool isResource;

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animationEvent = GetComponentInChildren<AnimationEventListener>();
        visualHandler = GetComponent<ActorVisualHandler>();
        animationEvent.attackEvent.AddListener(Attack);
        isResource = GetComponent<Resource>() ? true : false;
    }
    public void Update()
    {
        animator.SetFloat("Speed", Mathf.Clamp(agent.velocity.magnitude, 0, 1));
    }

    public void SetDestination(Vector3 destination)
    {
        agent.destination = destination;
    }
    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }
    void Attack()
    {
        if (damageableTarget)
            damageableTarget.Hit(0.350f);
    }
    public void AttackTarget(Damageable target)
    {
        //StopTask();
        damageableTarget = target;
        currentTask = StartCoroutine(StartAttack());

        IEnumerator StartAttack()
        {
            while (damageableTarget)
            {
                SetDestination(damageableTarget.transform.position);
                yield return WaitForNavMesh();
                while (damageableTarget && Vector3.Distance(damageableTarget.transform.position, transform.position) < 4f)
                {
                    if (damageableTarget)
                        animator.SetTrigger("Attack");
                    yield return new WaitForSeconds(1f);
                    
                }

            }

            currentTask = null;
        }
    }
    public virtual void StopTask()
    {
  
        damageableTarget = null;
        GetComponent<Builder>().currentBuilding = null;
        if (currentTask != null)
        {
            StopCoroutine(currentTask);
            
        }
    }

    private void OnMouseEnter()
    {
        isHover = true;
    }
    private void OnMouseExit()
    {
        isHover = false;
    }

}
