using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAttack : MonoBehaviour
{
    public int damagePerShot = 20;              
    public float timeBetweenAttacks = 10.0f;       

    public GameObject chick;  
    public AudioClip attackClip;

    PetMovement petMovement;
    AudioSource petAudio;
    float timer = 0.0f;    
    float distance;              

    void Awake()
    {
        petAudio = GetComponent<AudioSource>();
        petAudio.clip = attackClip;
        petMovement = GetComponent<PetMovement>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (petMovement.enemy != null) {
            distance = Vector3.Distance(transform.position, petMovement.enemy.transform.position);

            if (distance <= 10f && timer >= timeBetweenAttacks) {
                Shoot();
                timer = 0f;
            }
        }
    }

    void Shoot()
    {
        petAudio.Play();
        timer = 0f;

        Instantiate(chick, transform.position, transform.rotation);
    }
}
