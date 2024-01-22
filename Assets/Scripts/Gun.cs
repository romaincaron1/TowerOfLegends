using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20;
    public float fireRate = 2.0f;
    private float fireTimer;

    void FireCannon()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
    }

    void Update()
    {
        if(!GameManager.instance.isGameStarted)
        {
            return;
        }
        
        fireTimer += Time.deltaTime;
        GameObject nearestEnemy = FindNearestEnemy();
        if(fireTimer >= fireRate && nearestEnemy != null && bulletPrefab != null)
        {
            fireTimer = 0f;
            FireAtEnemy(nearestEnemy);
        }
        if(nearestEnemy != null)
        {
            RotateTowards(nearestEnemy.transform);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            
            if(distanceToEnemy < minDistance)
            {
                minDistance = distanceToEnemy;
                nearest = enemy;
            }
        }
        
        return nearest;
    }

    void FireAtEnemy(GameObject enemy)
    {
        Vector3 directionToEnemy = (enemy.transform.position - bulletSpawnPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = directionToEnemy * bulletSpeed;
    }

    void RotateTowards(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        directionToTarget.y = 0;
        if(directionToTarget == Vector3.zero)
        {
            return;
        }
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        
        Quaternion correction = Quaternion.Euler(0, 90, 0);
        
        lookRotation *= correction;

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision" + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("enemy"))
        {
            Destroy(gameObject);
        }
    }
}
