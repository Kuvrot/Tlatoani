using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Damageable))]
public class Actor : MonoBehaviour
{
    [HideInInspector]
    public NavMeshAgent agent;
    public bool isBuilder = true;
    public bool isArcher = false;
    [HideInInspector] public Damageable damageable;
    [HideInInspector] public Damageable damageableTarget;
    [HideInInspector] public Animator animator;
    [HideInInspector] public AnimationEventListener animationEvent;
    public Coroutine currentTask;
    [HideInInspector] public ActorVisualHandler visualHandler;
    bool canAttack = true;

    public bool isHover = false;
    bool isResource;
    float animSpeed = 0;

    Slider hpBar;


    //Stats  HP villager = 25 , Eagle warrior = 50 Jaguar Warrior = 65, Golem = 180
    //Stats Damage Villager = 0.350, Eagle warrior = 7, Jaguar warrior = 10 , Golem = 4.5
    //Stats speed Eagle warrior is the fastest like chavalry, Jaguar Warrior is slower than eagle warrior, Golem is very very slow.
    [Header("stats")]
    public float HP = 25;
    public float Damage = 0.350f;
    public float Speed = 3.5f;


    //Weakness
    // Eagle warrior strong against creatures and monsters, weak against distance enemies
    // Jaguar Warrior strong against distance enemies, weak with creatures
    //Golem strong to buildings, is weaknest is that walks very slow

    private void Awake()
    {
        damageable = GetComponent<Damageable>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animationEvent = GetComponentInChildren<AnimationEventListener>();
        visualHandler = GetComponent<ActorVisualHandler>();
        animationEvent.attackEvent.AddListener(Attack);
        isResource = GetComponent<Resource>() ? true : false;
        hpBar = GetComponentInChildren<Slider>();
        hpBar.maxValue = HP;

    }

    private void Start()
    {
        ActorManager.instance.allActors.Add(this);
        hpBar = GetComponentInChildren<Slider>();
        hpBar.maxValue = HP;

    }
    public void Update()
    {
        animSpeed = Mathf.Clamp(agent.velocity.magnitude, 0, 1);
        animator.SetFloat("Speed", animSpeed);
        
        hpBar.value = HP;

       if (damageableTarget != null)
        {
            if (damageableTarget.CompareTag("Enemy"))
            {
                SetDestination(damageableTarget.transform.position);
            }
        }

    }

    public void SetDestination(Vector3 destination)
    {

        agent.SetDestination(destination);
        agent.isStopped = false;
        

    }
    public WaitUntil WaitForNavMesh()
    {
        return new WaitUntil(() => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance);
    }
    void Attack()
    {
        if (damageableTarget)
        {

            if (damageableTarget.GetComponent<Enemy>())
            {
                damageableTarget.GetComponent<Enemy>().target = this.transform;
            }

            damageableTarget.Hit(Damage);

        }
            
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
                bool faceTarget = false;

                Vector3 targetPosition = target.transform.position;

                if (!target.CompareTag("Enemy"))
                {
                    
                    Vector2 randomPosition = Random.insideUnitCircle.normalized * 7;
                    targetPosition.x += randomPosition.x;
                    targetPosition.z += randomPosition.y;
                    
                }

                SetDestination(targetPosition);
                yield return WaitForNavMesh();
                while (damageableTarget && Vector3.Distance(targetPosition, transform.position) < agent.stoppingDistance + 2)
                {
                    if (damageableTarget)
                    {
                        if (canAttack)
                        {
                            if (!isArcher)
                            {
                                animator.SetTrigger("Attack");
                                if (!faceTarget)
                                {
                                    Vector3 lookdir = (damageableTarget.transform.position - transform.position).normalized;
                                    transform.rotation = Quaternion.LookRotation(lookdir);
                                    faceTarget = true;
                                }
                            }
                            else // in case de unit is an archer.
                            {
                                animator.SetTrigger("Bow");
                                GetComponent<Archer>().SpawnArrows(damageableTarget);
                                Vector3 dir = (damageableTarget.transform.position - transform.position).normalized;
                                transform.rotation = Quaternion.LookRotation(dir);
                            }

                            canAttack = false;
                        }
                    }
                    else
                    {
                        StopTask();
                        break;
                    }

                    if (isArcher)
                    {
                        yield return new WaitForSeconds(4f);
                    }

                    yield return new WaitForSeconds(0.5f);
                    canAttack = true;
                    
                }

            }

            currentTask = null;
            
        }
    }
    public virtual void StopTask()
    {

        animator.SetTrigger("Respawn");
        damageableTarget = null;
        GetComponent<Builder>().currentBuilding = null;
        if (currentTask != null)
        {
            StopCoroutine(currentTask);
            currentTask = null;
            
        }

    }

    public void GetDamage (float amount)
    {
        HP -= amount;

        if (HP <= 0)
        {
            StopTask();
            currentTask = null;
            ActorManager.instance.allActors.Remove(this);
            ActorManager.instance.selectedActors.Remove(this);
            Destroy(gameObject);
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
