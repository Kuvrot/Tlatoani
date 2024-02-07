using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int ID = 0; // 0 is Golem, 1 is Zombie 
    public Transform target;
    public float attackDistance = 3;
    bool canAttack = true;
    bool isDead = false;


    //Stats
    [Header("stats")]
    public float damage = 4;

    //Components
    NavMeshAgent nva;
    Animator anim;
    Damageable damageable;
    Slider healthBar;


    private void OnEnable()
    {
        nva = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        EnemyManager.instance.allEnemies.Add(this);
        nva.stoppingDistance = attackDistance - 3;
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = damageable.totalHealth;
        SearchTarget();
    }

    private void Update()
    {

        healthBar.value = damageable.currentHealth;

        if (damageable.currentHealth <= 0)
        {
            if (!isDead)
            {
                anim.SetTrigger("Death");
                isDead = true;
                nva.isStopped = true;
                StartCoroutine(Death());
            }
        }
        else
        {
            if (target == null)
            {
                SearchTarget();
            }
            else
            {
                Vector3 dir = (target.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(dir);

                float distance = Vector3.Distance(target.position, transform.position);

                if (distance <= attackDistance)
                {
                    if (canAttack)
                    {
                        StartCoroutine(Attacking());
                    }
                }
                else
                {
                    nva.SetDestination(target.position);
                }

            }
        }

    }


    IEnumerator Attacking ()
    {
        canAttack = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(3);
        canAttack = true;
        StopCoroutine(Attacking());
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(4);
        EnemyManager.instance.allEnemies.Remove(this);
        Destroy(gameObject);
    }

    void SearchTarget ()
    {
        int randomNumber = Random.Range(0 , ActorManager.instance.allActors.Count );

        target = ActorManager.instance.allActors[randomNumber].transform;
        nva.SetDestination(target.position);

    }

    public void Hit ()
    {
        target.GetComponent<Actor>().GetDamage(damage);
    }

}
