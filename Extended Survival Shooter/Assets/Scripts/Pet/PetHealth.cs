using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip deathClip;
    public Slider petHealthSlider;
    public TextMeshProUGUI petHealthText;

    Animator anim;
    // AudioSource playerAudio;
    PetMovement petMovement;
    PetAttack petAttack;
    bool isDead;                                                                                          


    void Awake()
    {
        anim = GetComponent<Animator>();
        // petAudio = GetComponent<AudioSource>();
        petMovement = GetComponent<PetMovement>();
        petAttack = GetComponentInChildren<PetAttack>();

        currentHealth = MainManager.Instance.currentPetHealth;
        petHealthSlider.value = currentHealth;
        petHealthText.text = currentHealth.ToString() + "/100";
    }


    void Update()
    {
        /*
        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
        */
    }


    public void TakeDamage(int amount)
    {

        // Reduce the current health by the amount of damage sustained.
        currentHealth -= amount;

        //Update the health slider
        petHealthSlider.value = currentHealth;
        // Update the health text
        petHealthText.text = currentHealth.ToString() + "/100";

        MainManager.Instance.currentPetHealth = currentHealth;

        // petAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        // Set pet in instance to ""
        MainManager.Instance.currentPet = "";
        
        isDead = true;

        // petAttack.DisableEffects();

        anim.SetBool("Run", false);
        anim.SetBool("Turn Head", true);

        // petAudio.clip = deathClip;
        // petAudio.Play();

        petMovement.enabled = false;
        petAttack.enabled = false;

        //After 2 seconds destory the enemy.
        Destroy(gameObject, 2f);
    }
}
