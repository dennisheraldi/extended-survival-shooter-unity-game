using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBuffMovement : MonoBehaviour
{
    public GameObject[] enemy;
    public float speed = 6f; 
    public float distanceToAvoidEnemy = 10f;
    Transform player;               // Reference to the player's position.
    PlayerHealth playerHealth;      // Reference to the player's health.
    PetBuffHealth petBuffHealth;    // Reference to this pet's health.
    EnemyHealth enemyHealth;        // Reference to this enemy's health.
    UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.

// Start is called before the first frame update
    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        petBuffHealth = GetComponent<PetBuffHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        // If the enemy and the player have health left...
        if (playerHealth.currentHealth <= 0 || petBuffHealth.currentHealth <= 0)
        {
            // avoid enemy position

            // ... set the destination of the nav mesh agent to the player.
            nav.enabled = false;
            return;
        }

        else
        {
            foreach (GameObject enemy in enemy)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < distanceToAvoidEnemy)
                {
                    Vector3 directionToEnemy = transform.position - enemy.transform.position;
                    Vector3 newPosition = transform.position + directionToEnemy.normalized * speed * Time.deltaTime;
                    nav.SetDestination(newPosition);
                }
                    else
                {
                    // Otherwise, set the destination of the nav mesh agent to the player.
                    nav.SetDestination(player.transform.position);
                }
            }
        }

        // else
        // {
        //     // ... disable the nav mesh agent.
        //     nav.enabled = false;
        // }
    }
}
