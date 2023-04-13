using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    GameObject pet;
    PlayerHealth playerHealth;
    PetBuffHealth petHealth;
    //EnemyHealth enemyHealth;
    bool playerInRange;
    bool petInRange;
    float timer;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        pet = GameObject.FindGameObjectWithTag ("Pet");
        petHealth = pet.GetComponent <PetBuffHealth> ();
        // enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player && other.isTrigger == false)
        {
            playerInRange = true;
        }
        if (other.gameObject == pet && other.isTrigger == false)
        {
            petInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
        if (other.gameObject == pet)
        {
            petInRange = false;
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && MainManager.Instance.immunity == false/* && enemyHealth.currentHealth > 0*/)
        {
            AttackPlayer ();
        }
        if (timer >= timeBetweenAttacks && petInRange/* && enemyHealth.currentHealth > 0*/)
        {
            AttackPet ();
        }

        if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger ("PlayerDead");
        }
    }


    void AttackPlayer ()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.TakeDamage (attackDamage);
        }

    }

    void AttackPet ()
    {
        timer = 0f;

        if (petHealth.currentHealth > 0)
        {
            petHealth.TakeDamage (attackDamage);
        }
    }
}
