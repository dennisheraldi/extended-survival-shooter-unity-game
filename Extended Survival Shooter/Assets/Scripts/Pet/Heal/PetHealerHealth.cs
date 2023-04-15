using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetHealerHealth : MonoBehaviour
{
    public int startingHealth = 100;            // The amount of health the enemy starts the game with.
    public int currentHealth;                   // The current health the enemy has.
    public float sinkSpeed = 2.5f;              // The speed at which the enemy sinks through the floor when dead.            // The sound to play when the enemy dies.

    public Slider petHealthSlider;
    public TextMeshProUGUI petHealthText; 


    Animator anim;                              // Reference to the animator.
    CapsuleCollider capsuleCollider;            // Reference to the capsule collider.
    bool isDead;                                // Whether the enemy is dead.
    bool isSinking;                             // Whether the enemy has started sinking through the floor.


    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Setting the current health when the enemy first spawns.
        currentHealth = startingHealth;
    }


    void Update()
    {
        // If the enemy should be sinking...
        if (isSinking)
        {
            // ... move the enemy down by the sinkSpeed per second.
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage(int amount)
    {
        // If the enemy is dead...
        if (isDead)
        {
            return;
        }

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;

        // Update the health slider
        // petHealthSlider.value = currentHealth;
        // // Update the health text
        // petHealthText.text = currentHealth.ToString() + "/100";

        // MainManager.Instance.currentPetHealth = currentHealth;

        // If the current health is less than or equal to zero...
        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            Death();
        }
    }


    void Death()
    {
        // Set pet in instance to ""
        MainManager.Instance.currentPet = "";

        // The enemy is dead.
        isDead = true;

        // Turn the collider into a trigger so shots can pass through it.
        capsuleCollider.isTrigger = true;

        // Tell the animator that the enemy is dead.
        anim.SetTrigger("Die");
    }


    public void StartSinking()
    {
        // Find and disable the Nav Mesh Agent. 
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

        // Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
        GetComponent<Rigidbody>().isKinematic = true;

        // The enemy should no sink.
        isSinking = true;

        // After 2 seconds destory the enemy.
        Destroy(gameObject, 2f);
    }
}
