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
    [SerializeField] float totalHealth = 100;
    public float currentHealth;
    private void Start()
    {
        currentHealth = 1;

        isBuilding = GetComponent<Building>() ? true : false;

    }

    public void Hit(float damage)
    {
        if (!isBuilding)
        {
            onHit.Invoke();
            currentHealth -= damage;
            if (currentHealth <= 0)
                Destroy();
        }
    }
    void Destroy()
    {
        onDestroy.Invoke();
        Destroy(gameObject);
    }
}
