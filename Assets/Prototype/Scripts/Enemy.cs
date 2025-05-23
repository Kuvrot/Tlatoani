﻿using System.Collections;
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


    //Stats
    [Header("stats")]
    public float damage = 4;

    //Components
    NavMeshAgent nva;
    Animator anim;
    Damageable damageable;
    Slider healthBar;
    new SkinnedMeshRenderer renderer;


    private void OnEnable()
    {
        nva = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        nva.stoppingDistance = attackDistance - 3;
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = damageable.totalHealth;
        renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        SearchTarget();
    }

    private void Update()
    {

        healthBar.value = damageable.currentHealth;

        if (damageable.currentHealth <= 0)
        {
            //if (!isDead)
            //{
                //anim.SetTrigger("Death");
                EnemyManager.instance.allEnemies.Remove(this.gameObject);
                StartCoroutine(Death());
            //}
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
                    nva.isStopped = true;

                    if (canAttack)
                    {
                        StartCoroutine(Attacking());
                    }
                }
                else
                {
                    nva.isStopped = false;
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
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    void SearchTarget ()
    {
        int randomNumber = Random.Range(0 , ActorManager.instance.allActors.Count );

        target = ActorManager.instance.allActors[randomNumber].transform;
        nva.SetDestination(target.position);

    }

    public void Hit ()
    {
        Actor actor;
        actor = target.GetComponent<Actor>();

        actor.GetDamage(damage);

        if (actor.damageableTarget == null && !actor.isBuilder)
        {
            actor.AttackTarget(this.GetComponent<Damageable>());
        }



    }

    private void OnMouseEnter() { 
    
        if (renderer)
            renderer.material.SetColor("_EmissionColor", Color.gray);
    }
    private void OnMouseExit()
    {
        if (renderer)
            renderer.material.SetColor("_EmissionColor", Color.black);
    }

}
