using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBuffAction : MonoBehaviour
{
    public float timeBetweenBuff = 0.5f;
    public int buffDamage = 1000;


    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    //EnemyHealth enemyHealth;
    float timer;
    public bool buffed;
    ParticleSystem buffEffect;


    void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        buffEffect = player.GetComponentsInChildren<ParticleSystem>()[2];
        //enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }

    // void OnTriggerEnter (Collider other)
    // {
    //     if(other.gameObject == player)
    //     {
    //         playerInRange = true;
    //     }
    // }

    // void OnTriggerExit (Collider other)
    // {
    //     if(other.gameObject == player)
    //     {
    //         playerInRange = false;
    //     }
    // }


    void Update ()
    {
        float distanceWithPlayer = Vector3.Distance (player.transform.position, transform.position);

        if (distanceWithPlayer < 2f)
        {
            buffEffect.Play();
            anim.SetBool("IsInRange", true);
            buffed = true;
        }
        else
        {
            anim.SetBool("IsInRange", false);
            buffed = false;
        }
    }
}
