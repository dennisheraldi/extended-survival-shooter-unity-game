using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetHealerAction : MonoBehaviour
{
    public float timeBetweenHeal = 0.5f;
    public int healValue = 10;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    float timer;
    public bool healed;
    UnityEngine.AI.NavMeshAgent nav;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        //enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    }



    void Update ()
    {
        float distanceWithPlayer = Vector3.Distance (player.transform.position, transform.position);

        if (distanceWithPlayer < 2f)
        {
            anim.SetBool("IsInRange", true);
            HealPlayer();
            nav.enabled = false;
        }
        else
        {
            anim.SetBool("IsInRange", false);
        }
    }

    void HealPlayer ()
    {

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.HealPlayer(healValue);
        }

    }
}
