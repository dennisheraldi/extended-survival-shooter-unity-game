﻿using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.
    public float range = 100f;                      // The distance the gun can fire.                  

    float timer;                                    // A timer to determine when to fire.
    Ray shootRay = new Ray();                       // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    AudioSource gunAudio;                           // Reference to the audio source.
    Light gunLight;                                 // Reference to the light component.
    //public Light faceLight;                             // Duh
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.             
    public PauseManager pauseManager;
    PetBuffAction petBuffAction;
    ParticleSystem BuffParticles;
    GameObject pet;
    PetBuffHealth petBuffHealth;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        //faceLight.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;
        //faceLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.enabled = true;
        gunLine.SetPosition(0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                if (MainManager.Instance.instantKill == true)
                {
                    enemyHealth.TakeDamage(1000, shootHit.point);
                }
                else
                {
                    Debug.Log("Halo");
                    if (GameObject.FindGameObjectWithTag("Buff") != null)
                    {
                        Debug.Log("Buffed");
                        pet = GameObject.FindGameObjectWithTag("Buff");
                        petBuffAction = pet.GetComponent<PetBuffAction>();
                        if (petBuffAction.buffed == true)
                        {
                            Debug.Log("Buffed");
                            int damageBuff = petBuffAction.buffDamage;
                            enemyHealth.TakeDamage(damageBuff, shootHit.point);
                        }
                        else
                        {
                            enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                        }

                    }
                    else
                    {
                        enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                    }
                }
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}