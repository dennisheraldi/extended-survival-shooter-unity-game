using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    BoxCollider boxCollider;
    bool hit = false;
    public float time;
    float timer;

    void Awake() {
        timer = 0f;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update() {
        timer += Time.deltaTime;
        if (hit || timer >= time + 0.5f) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy") && collision.collider.GetType() == typeof(CapsuleCollider)) {
            EnemyHealth enemyHealth = collision.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                if (MainManager.Instance.instantKill == true)
                {
                    enemyHealth.TakeDamage(1000, collision.GetContact(0).point);
                }
                else 
                {
                    if (GameObject.FindGameObjectWithTag("Buff") != null)
                    {
                        GameObject pet = GameObject.FindGameObjectWithTag("Buff");
                        PetBuffAction buff = pet.GetComponent<PetBuffAction>();
                        if (buff.buffed)
                        {
                                int damageBuff = buff.buffDamage;
                                enemyHealth.TakeDamage(damageBuff, collision.GetContact(0).point);
                        }
                        else
                        {
                            int damage = 20;
                            enemyHealth.TakeDamage(damage, collision.GetContact(0).point);
                        }
                    }
                    else
                    {
                        int damage = 20;
                        enemyHealth.TakeDamage(damage, collision.GetContact(0).point);
                    }
                }

            }
            boxCollider.enabled = false;
            hit = true;
        }
    }
}
