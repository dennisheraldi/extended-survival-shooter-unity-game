using UnityEngine;

public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
        PlayerHealth playerHealth;      // Reference to the player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        PetBuffHealth PetBuffHealth;    // Reference to this pet's health.
        UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.
        Transform pet;


        void Awake()
        {
            // Set up the references.
            player = GameObject.FindGameObjectWithTag("Player").transform;
            pet = GameObject.FindGameObjectWithTag("Pet").transform;

            playerHealth = player.GetComponent<PlayerHealth>();
            PetBuffHealth = pet.GetComponent<PetBuffHealth>();

            enemyHealth = GetComponent<EnemyHealth>();
            nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }


        void Update()
        {
            // If the enemy and the player have health left...
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);


            if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                if (PetBuffHealth.currentHealth > 0)
                {
                    float distanceToPet = Vector3.Distance(transform.position, pet.position);
                    if (distanceToPlayer < distanceToPet)
                    {
                        // ... set the destination of the nav mesh agent to the player.
                        nav.SetDestination(player.position);
                    }
                    else
                    {
                        // ... set the destination of the nav mesh agent to the pet.
                        nav.SetDestination(pet.position);
                    }
                }
                else
                {
                    nav.SetDestination(player.position);
                }
                // ... set the destination of the nav mesh agent to the player.
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                nav.enabled = false;
            }
        }
    }