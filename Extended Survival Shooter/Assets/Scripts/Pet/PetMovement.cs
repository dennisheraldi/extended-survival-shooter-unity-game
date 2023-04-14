using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetMovement : MonoBehaviour
{
    public GameObject enemy;

    Transform player;
    PlayerHealth playerHealth;
    PetHealth petHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    Animator anim;

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        petHealth = GetComponent<PetHealth>();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent>();
    }


    void Update ()
    {
        // find first targeted enemy
        if (enemy == null || (enemy != null && enemyHealth.currentHealth <= 0)) {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (enemy != null) {
                enemyHealth = enemy.GetComponent<EnemyHealth>();
            }
        }
        
        else if (enemy != null) {
            if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && petHealth.currentHealth > 0 ) {
                // todo: set rotation
                nav.SetDestination(enemy.transform.position);
                if (nav.velocity.x == 0 && nav.velocity.z == 0) {
                    anim.SetBool("Run", false);
                } else {
                    anim.SetBool("Run", true);
                }
            }
            // player dead
            else {
                nav.enabled = false;
                anim.SetBool("Run", false);
            }
        }
    }
}