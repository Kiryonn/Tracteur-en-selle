using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float health { get; private set; }
    [SerializeField] float maxHealth;
    Rigidbody rb;
    private void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("It's colliding with : "+collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Hazard"))
        {
            DamageTractor(1f);
        }
    }

    public void DamageTractor(float amount)
    {
        health -= amount;
        Debug.Log("Endommagement "+rb.velocity);
    }

    public void HealTractor(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
}
