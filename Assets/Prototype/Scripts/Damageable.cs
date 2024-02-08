using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [HideInInspector]
    public bool isBuilding;
    public UnityEvent onDestroy = new UnityEvent();
    public UnityEvent onHit = new UnityEvent();
    [SerializeField] public float totalHealth = 100;
    public float currentHealth;

    //Components
    AudioSource audioSource;

    private void Start()
    {

        isBuilding = GetComponent<Building>() ? true : false;
        audioSource = GetComponent<AudioSource>();
        if (isBuilding)
        {
            currentHealth = 1;
        }
        else
        {
            currentHealth = totalHealth;
        }

    }

    public void Hit(float damage)
    {
        if (!isBuilding)
        {
            onHit.Invoke();
            currentHealth -= damage;

            audioSource.Play();
        }

    }
}
