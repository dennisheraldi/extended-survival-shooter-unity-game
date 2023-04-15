using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetHealerMovement : MonoBehaviour
{
    public float speed = 6f; 
    Transform player;               // Reference to the player's position.
    PlayerHealth playerHealth;      // Reference to the player's health.
    PetHealerHealth petHealerHealth;    // Reference to this pet's health.
    EnemyHealth enemyHealth;        // Reference to this enemy's health.
    UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.

// Start is called before the first frame update
    void Awake()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        petHealerHealth = GetComponent<PetHealerHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }


    void Update()
    {
        // If the enemy and the player have health left...
        if (playerHealth.currentHealth <= 0 || petHealerHealth.currentHealth <= 0)
        {
            // avoid enemy position

            // ... set the destination of the nav mesh agent to the player.
            nav.enabled = false;
            return;
        }

        else
        {
            nav.enabled = true;
            nav.SetDestination(player.transform.position);
        }

    }
}
