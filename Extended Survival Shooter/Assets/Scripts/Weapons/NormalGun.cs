using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : MonoBehaviour
{
    public int damagePerShot = 20;              
    public float timeBetweenBullets = 0.15f;       
    public float range = 100f;

    float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;                             
    ParticleSystem gunParticles;           
    LineRenderer gunLine;                        
    AudioSource gunAudio;                           
    Light gunLight;                                 
    float effectsDisplayTime = 0.2f;

    GameObject gunBarrel;

    // scene manager
    // public PauseManager pauseManager;

    void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");

        gunBarrel = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        gunParticles = gunBarrel.GetComponent<ParticleSystem>();
        gunLine = gunBarrel.GetComponent<LineRenderer>();
        gunAudio = gunBarrel.GetComponent<AudioSource>();
        gunLight = gunBarrel.GetComponent<Light>();
        gunLine.enabled = false;
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire... (+ scene manager)
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets /* && !pauseManager.canvas.enabled */)
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
        gunLine.SetPosition(0, gunBarrel.transform.position);

        shootRay.origin = gunBarrel.transform.position;
        shootRay.direction = gunBarrel.transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                // cheat?
                //if (MainManager.Instance.instantKill == true)
                //{
                //    enemyHealth.TakeDamage(1000, shootHit.point);
                //}
                //else
                //{
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                //}
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }
}
