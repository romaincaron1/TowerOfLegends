using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Nécessaire pour accéder aux fonctionnalités de NavMesh

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent; // Référence à l'agent de navigation
    private Transform target; // La cible actuelle

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Récupération du composant NavMeshAgent
        target = FindNearestDefenseObject(); // Trouver la cible la plus proche au démarrage
    }

    void Update()
    {
        // Si nous avons une cible, déplacer l'ennemi vers cette cible
        if(target != null)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            // Si la cible est nulle, tenter de trouver une nouvelle cible
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
}
