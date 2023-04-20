using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float health { get; private set; }
    public float maxHealth;
    Rigidbody rb;

    [SerializeField] SkinnedMeshRenderer[] damageableParts;
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
            DamageTractor(5f);
        }

        if (collision.gameObject.layer == 7)
        {
            DamageTractor(25f);
        }
    }

    public void DamageTractor(float amount)
    {
        health -= amount;
        Debug.Log("Endommagement "+rb.velocity);
        UpdateVisualDamage();
    }

    public void HealTractor(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        UpdateVisualDamage();
    }

    void UpdateVisualDamage()
    {
        for (int i = 0; i<damageableParts.Length; i++)
        {
            Debug.Log("Setting blendshape values to "+ (1 - health / maxHealth) * 100);
            damageableParts[i].SetBlendShapeWeight(0, (1 - health / maxHealth) * 100);
        }
    }
}
