using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAttack : MonoBehaviour
{
    public int damagePerShot = 20;              
    public float timeBetweenAttacks = 10.0f;       

    public GameObject chick;  

    PetMovement petMovement;
    float timer = 0.0f;    
    float distance;              

    void Awake()
    {
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
        timer = 0f;

        Instantiate(chick, transform.position, transform.rotation);
    }
}
