using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickAttack : MonoBehaviour
{
    PetMovement petMovement;   
    GameObject pet;
    bool hit = false;        

    void Start()
    {
        pet = GameObject.FindGameObjectWithTag("Pet");
        petMovement = pet.GetComponent<PetMovement>();
    }

    void Update()
    {
        if (hit) {
            Destroy(gameObject);
            return;
        }
        if (petMovement.enemy != null) {
            Vector3 enemyPosition = petMovement.enemy.transform.position;
            Vector3 enemyDirection = enemyPosition - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, enemyPosition, 0.1f);
            Vector3 enemyRotation = Vector3.RotateTowards(transform.forward, enemyDirection, Mathf.PI/4, 0.0f);
            transform.rotation = Quaternion.LookRotation(enemyRotation);

        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && collision.collider.GetType() == typeof(CapsuleCollider)) {
            EnemyHealth enemyHealth = collision.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                int damage = 50;
                enemyHealth.TakeDamage(damage, collision.GetContact(0).point);
            }
            hit = true;
        }
    }
}
