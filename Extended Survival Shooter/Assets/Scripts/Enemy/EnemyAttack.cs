using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;


    Animator anim;
    GameObject player;
    GameObject pet;
    GameObject healer;
    PlayerHealth playerHealth;
    PetBuffHealth petHealth;
    PetHealerHealth healerHealth;
    //EnemyHealth enemyHealth;
    bool playerInRange;
    bool petInRange;
    bool healerInRange;
    float timer;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        // enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
        timer = 0f;
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player && other.isTrigger == false)
        {
            playerInRange = true;
        }
        if (GameObject.FindGameObjectWithTag("Buff") != null)
        {
            pet = GameObject.FindGameObjectWithTag("Buff");
            if (other.gameObject == pet && other.isTrigger == false)
            {
                petInRange = true;
            }
        }
        if (GameObject.FindGameObjectWithTag("Healer") != null)
        {
            healer = GameObject.FindGameObjectWithTag("Healer");
            if (other.gameObject == healer && other.isTrigger == false)
            {
                healerInRange = true;
            }
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
        if (GameObject.FindGameObjectWithTag("Buff") != null)
        {
            pet = GameObject.FindGameObjectWithTag("Buff");
            if (other.gameObject == pet)
            {
                petInRange = false;
            }
        }
        if (GameObject.FindGameObjectWithTag("Healer") != null)
        {
            healer = GameObject.FindGameObjectWithTag("Healer");
            if (other.gameObject == healer)
            {
                healerInRange = false;
            }
        }
    }


    void Update ()
    {
        timer += Time.deltaTime;

        if(timer >= timeBetweenAttacks && playerInRange && MainManager.Instance.immunity == false/* && enemyHealth.currentHealth > 0*/)
        {
            AttackPlayer ();
        }
        if (GameObject.FindGameObjectWithTag("Buff") != null)
        {
            if (timer >= timeBetweenAttacks && petInRange/* && enemyHealth.currentHealth > 0*/)
            {
                AttackPet ();
            }
        }
        if (GameObject.FindGameObjectWithTag("Healer") != null)
        {
            if (timer >= timeBetweenAttacks && healerInRange/* && enemyHealth.currentHealth > 0*/)
            {

                AttackHealer ();
            }
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
        petHealth = GameObject.FindGameObjectWithTag("Buff").GetComponent<PetBuffHealth>();
        if (petHealth.currentHealth > 0)
        {
            petHealth.TakeDamage (attackDamage);
        }


    }

    void AttackHealer()
    {
        timer = 0f;
        healerHealth = GameObject.FindGameObjectWithTag("Healer").GetComponent<PetHealerHealth>();
        if (healerHealth.currentHealth > 0)
        {
            healerHealth.TakeDamage (attackDamage);
        }
    }
}
