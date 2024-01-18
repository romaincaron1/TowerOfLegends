using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulet : MonoBehaviour
{
    public int damage = 1;
    
    void Awake()
    {
        Destroy(gameObject, 3);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
