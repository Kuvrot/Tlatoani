using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 20;
    public Vector3 target;

    // Update is called once per frame
    void Update()
    {
         if (target != Vector3.zero)
        {
            Vector3 dir = (target - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(dir);
        }       
    }

    public void SetTarget (Transform target)
    {
        this.target = target.position;


    }
}
