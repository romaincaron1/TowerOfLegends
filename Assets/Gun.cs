using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20;
    public float fireRate = 2.0f; // Temps en secondes entre chaque tir
    private float fireTimer; // Un compteur pour le rythme de tir

    void FireCannon()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
    }

    void Update()
    {
        fireTimer += Time.deltaTime;
        GameObject nearestEnemy = FindNearestEnemy();
        if(fireTimer >= fireRate)
        {
            fireTimer = 0f;
            
            
            if(nearestEnemy != null)
            {
                FireAtEnemy(nearestEnemy);
            }
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
    Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
    
    // Créez une rotation de 90 degrés autour de l'axe Y
    Quaternion correction = Quaternion.Euler(0, 90, 0);
    
    // Appliquez d'abord la correction puis la rotation vers la cible
    lookRotation *= correction;

    // Appliquez la rotation corrigée au canon
    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
}

}
