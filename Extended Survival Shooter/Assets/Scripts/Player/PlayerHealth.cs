using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;                            // The amount of health the player starts the game with.
    public int currentHealth;                                   // The current health the player has.
    public TextMeshProUGUI healthText;
    public Slider healthSlider;                                 // Reference to the UI's health bar.
    public Image damageImage;                                   // Reference to an image to flash on the screen on being hurt.
    public Image healImage;                                   // Reference to an image to flash on the screen on being hurt.
    public AudioClip deathClip;                                 // The audio clip to play when the player dies.
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.
    
    public static float startTime;
    public static float deathTime;

    Animator anim;                                              // Reference to the Animator component.
    AudioSource playerAudio;                                    // Reference to the AudioSource component.
    PlayerMovement playerMovement;                              // Reference to the player's movement.
    PlayerShooting playerShooting;                              // Reference to the PlayerShooting script.
    public static bool isDead;                                                // Whether the player is dead.
    bool damaged;                                               // True when the player gets damaged.
    
    ParticleSystem HealParticles;                // Reference to the particle system that plays when the enemy is damaged.
    GameObject gunObject;
    PlayerShooting gun;
    
    

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponentInChildren<PlayerShooting>();
        HealParticles = GetComponentsInChildren<ParticleSystem>()[3];

        gunObject = GameObject.Find("GunBarrelEnd");
        gun = gunObject.GetComponent<PlayerShooting>();
        
        // Set the initial health of the player.
        currentHealth = MainManager.Instance.currentPlayerHealth;
        startTime = Time.time;
        gunObject = GameObject.Find("GunBarrelEnd");

        healthSlider.value = currentHealth;
        healthText.text = currentHealth.ToString() + "/100";
    }


    void Update()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
        if (QuestManager.TimerQ3.ToString("0.00") == "0.00")
        {
            Death();
        }
    }

    public void HealPlayer(int amount)
    {
        // Reduce the current health by the damage amount.
        if (currentHealth + amount > 100)
        {
            currentHealth = 100;
        }
        else
        {
            currentHealth += amount;
        }

        MainManager.Instance.currentPlayerHealth = currentHealth;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Set the health text 
        healthText.text = currentHealth.ToString() + "/100";
        HealParticles.Play();
    }


    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        damaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        MainManager.Instance.currentPlayerHealth = currentHealth;

        // Set the health bar's value to the current health.
        healthSlider.value = currentHealth;

        // Set the health text 
        healthText.text = currentHealth.ToString() + "/100";

        // Play the hurt sound effect.
        playerAudio.Play();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
  
    }


    public void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        deathTime = Time.time;

        // Turn off any remaining shooting effects.
        playerShooting.DisableEffects();

        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // Turn off the movement and shooting scripts.
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    /*public void RestartLevel()
    {
        // Reload the level that is currently loaded.
        // LastSavedScene 
        PlayerHealth.isDead = false;
        SceneManager.LoadScene("MainScene");
    }*/
}