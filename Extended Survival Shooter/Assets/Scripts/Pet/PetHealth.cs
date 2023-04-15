using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public AudioClip deathClip;
    public Slider petHealthSlider;

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

        currentHealth = startingHealth;
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

        currentHealth -= amount;

        petHealthSlider.value = currentHealth;

        // petAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        // petAttack.DisableEffects();

        anim.SetBool("Run", false);
        anim.SetBool("Turn Head", true);

        // petAudio.clip = deathClip;
        // petAudio.Play();

        petMovement.enabled = false;
        petAttack.enabled = false;
    }
}
