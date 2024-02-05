using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Transform target;
    public float Health = 180;
    public float attackDistance = 3;
    bool canAttack = true;


    //Components
    NavMeshAgent nva;
    Animator anim;


    private void OnEnable()
    {
        nva = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        nva.stoppingDistance = attackDistance - 3;
        SearchTarget();
    }

    private void Update()
    {
      

        if (target == null)
        {
            SearchTarget();
        }
        else
        {
            Vector3 dir = (target.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir);

            float distance = Vector3.Distance(target.position , transform.position);

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

        if (Health <= 0)
        {
            anim.SetTrigger("Death");
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

    void SearchTarget ()
    {
        int randomNumber = Random.Range(0 , ActorManager.instance.allActors.Count );

        target = ActorManager.instance.allActors[randomNumber].transform;
        nva.SetDestination(target.position);

    }

}
