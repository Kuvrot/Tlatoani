using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Actor
{
   public Building currentBuilding;
    private void Start()
    {
        animationEvent.attackEvent.AddListener(DoWork);
    }
    public void GiveJob(Building job)
    {

        if (!isBuilder)
        {
            return;
        }

        //StopTask();
        currentBuilding = job;
        currentTask = StartCoroutine(StartJob());
        IEnumerator StartJob()
        {
            if (currentBuilding != null)
            {

                Vector3 jobPosition = job.transform.position;
                Vector2 randomPosition = Random.insideUnitCircle.normalized * currentBuilding.radius;
                jobPosition.x += randomPosition.x;
                jobPosition.z += randomPosition.y;
                SetDestination(jobPosition);
                yield return WaitForNavMesh();

                if (currentBuilding != null)
                {
                    job = null;
                    agent.isStopped = true;
                    Vector3 dir = (currentBuilding.transform.position - transform.position).normalized;
                    transform.rotation = Quaternion.LookRotation(dir);
                }


                while (!currentBuilding.IsFinished())
                {
                    if (currentBuilding == null)
                    {
                        StopTask();
                        break;
                    }

                    if (!currentBuilding.IsFinished() && currentBuilding != null)
                        animator.SetTrigger("Attack");
                    else
                    {
                        StopTask();
                        break;
                    }

                    yield return new WaitForSeconds(0.5f);
                }

            }

            StopTask();
            currentBuilding = null;
            currentTask = null;
        }
    }
    public bool HasTask()
    {
        return currentTask != null;
    }
    override public void StopTask()
    {
        base.StopTask();
        currentBuilding = null;
        currentTask = null;
    }

    void DoWork()
    {
        if (currentBuilding)
            currentBuilding.Build(25);
    }
}
