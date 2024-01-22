using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    [SerializeField] float health, maxHealth = 3f;
    [SerializeField] FloatingHealthBar healthBar;

    void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        
            health = maxHealth;
            healthBar.UpdateHealthBar(health, maxHealth);
            agent = GetComponent<NavMeshAgent>();
            target = FindNearestDefenseObject();
        
    }

    void Update()
    {
        if(!GameManager.instance.isGameStarted)
        {
            return;
        }
        if(target != null && agent != null)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            target = FindNearestDefenseObject();
        }
    }

    Transform FindNearestDefenseObject()
    {
        GameObject[] defenses = GameObject.FindGameObjectsWithTag("defense");
        Transform nearestDefense = null;
        float closestDistance = Mathf.Infinity;

        foreach(GameObject defense in defenses)
        {
            float distance = Vector3.Distance(transform.position, defense.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                nearestDefense = defense.transform;
            }
        }

        return nearestDefense;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
