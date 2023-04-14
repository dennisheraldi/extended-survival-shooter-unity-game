using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    BoxCollider boxCollider;
    bool hit = false;

    void Start() {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update() {
        if (hit) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && collision.collider.GetType() == typeof(CapsuleCollider)) {
            EnemyHealth enemyHealth = collision.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                int damage = 20;
                enemyHealth.TakeDamage(damage, collision.GetContact(0).point);
            }
            boxCollider.enabled = false;
            hit = true;
        }
    }
}
