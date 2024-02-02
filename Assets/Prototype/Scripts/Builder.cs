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
        //StopTask();
        currentBuilding = job;
        currentTask = StartCoroutine(StartJob());
        IEnumerator StartJob()
        {
            Vector3 jobPosition = job.transform.position;
            Vector2 randomPosition = Random.insideUnitCircle.normalized * currentBuilding.radius;
            jobPosition.x += randomPosition.x;
            jobPosition.z += randomPosition.y;
            SetDestination(jobPosition);
            yield return WaitForNavMesh();
            Vector3 dir = (currentBuilding.transform.position - transform.position).normalized;
            transform.LookAt(dir);
            while (!currentBuilding.IsFinished())
            {
                yield return new WaitForSeconds(0.5f);
                if (!currentBuilding.IsFinished())
                    animator.SetTrigger("Attack");
                else
                    StopTask();
            }
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
    }

    void DoWork()
    {
        if (currentBuilding)
            currentBuilding.Build(10);
    }
}
