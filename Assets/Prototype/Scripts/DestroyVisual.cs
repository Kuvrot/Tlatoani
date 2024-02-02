using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVisual : MonoBehaviour
{
    Damageable damageable;
    public GameObject destroyParticle;
    void Start()
    {
        damageable = GetComponent<Damageable>();
        damageable.onDestroy.AddListener(Destroy);
    }

    public void Destroy()
    {
        if(Application.isPlaying)
            Instantiate(destroyParticle, transform.position + Vector3.up, Quaternion.identity);
    }
}
