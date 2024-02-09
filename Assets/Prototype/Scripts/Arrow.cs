using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 20;
    public Vector3 target;

    [HideInInspector]
    public bool fromBuilding = false;

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
        if (target == null)
        {
            Destroy(gameObject);
        }

        this.target = target.position;
        
        StartCoroutine(Timer());

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")){

            collision.gameObject.GetComponent<Damageable>().Hit(5);
            
            if (!fromBuilding)
            {
                collision.gameObject.GetComponent<Enemy>().target = transform;
            }

            Destroy(gameObject);

        }
    }

    IEnumerator Timer()
    {

        yield return new WaitForSeconds(4);
        Destroy(gameObject);
        StopAllCoroutines();

    }

}
