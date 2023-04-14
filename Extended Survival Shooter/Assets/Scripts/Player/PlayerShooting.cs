using UnityEngine;
using System.Collections.Generic;

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

    // scene manager      
    public PauseManager pauseManager;

    // shotgun
    List<LineRenderer> gunLines;       
    float bulletSpread = 0.2f;     
    int bullets = 5;
    int bulletVariation = 2;    
    float rangeShotgun = 5f;     

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

        // shotgun
        gunLines = new List<LineRenderer>();
        
        for (int i = 0; i < (bullets + bulletVariation) * 2; i++) {
            LineRenderer gunLine = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
            gunLines.Add(gunLine);
            if (i < bullets + bulletVariation) {
                gunLine.name = "GunLine" + i;
            } else {
                gunLine.name = "GunLineExtra" + (i - (bullets + bulletVariation));
            }
            gunLine.startWidth = 0.05f;
            gunLine.endWidth = 0.05f;
            gunLine.material = new Material(Shader.Find("Sprites/Default"));
        }
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire... (+ scene manager)
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && !pauseManager.canvas.enabled)
        {
            ShootShotgun();
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

        // shotgun
        for (int i = 0; i < gunLines.Count; i++) {
            gunLines[i].enabled = false;
        }
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
                // cheat?
                if (MainManager.Instance.instantKill == true)
                {
                    enemyHealth.TakeDamage(1000, shootHit.point);
                }
                else
                {
                enemyHealth.TakeDamage(damagePerShot, shootHit.point);
                }
            }

            gunLine.SetPosition(1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }

    void ShootShotgun() {
        // shotgun
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        int bulletsShot = Random.Range(gunLines.Count/2 - 2 * bulletVariation, gunLines.Count/2 - bulletVariation);

        for (int i = 0; i < bulletsShot; i++) {
            gunLines[i].enabled = true;
            gunLines[i].startColor = Color.red;
            gunLines[i].endColor = Color.green;
            gunLines[i].SetPosition(0, transform.position);

            shootRay.origin = transform.position;
            shootRay.direction = (transform.forward + new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0)).normalized;

            if (Physics.Raycast(shootRay, out shootHit, rangeShotgun, shootableMask))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    int damage = (int)(damagePerShot/((shootHit.point - shootRay.origin).magnitude * (shootHit.point - shootRay.origin).magnitude));
                    enemyHealth.TakeDamage(damage, shootHit.point);
                }

                gunLines[i].SetPosition(1, shootHit.point);
            }
            else
            {
                gunLines[i].SetPosition(1, shootRay.origin + shootRay.direction * rangeShotgun);
                gunLines[i + bulletsShot].enabled = true;
                gunLines[i + bulletsShot].SetPosition(0, shootRay.origin + shootRay.direction * rangeShotgun);
                gunLines[i + bulletsShot].SetPosition(1, shootRay.origin + shootRay.direction * 100f);
                gunLines[i + bulletsShot].startColor = Color.green;
                gunLines[i + bulletsShot].endColor = Color.green;
            }
        }
    }
}