using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVisualEffect : MonoBehaviour
{

    ParticleSystem ps;


    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!ps.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
