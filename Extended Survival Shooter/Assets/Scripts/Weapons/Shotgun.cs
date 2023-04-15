using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public int damagePerShot = 15;              
    public float timeBetweenBullets = 0.5f;       

    float timer;                                    
    Ray shootRay;                                   
    RaycastHit shootHit;                            
    int shootableMask;                             
    ParticleSystem gunParticles;                                
    AudioSource gunAudio;                           
    Light gunLight;                                 
    float effectsDisplayTime = 0.2f;

    Animator anim;

    GameObject gunBarrel;

    List<LineRenderer> gunLines;       
    float bulletSpread = 0.2f;     
    int bullets = 5;
    int bulletVariation = 2;    
    float range = 5f;   

    // scene manager
    // public PauseManager pauseManager;

    void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");

        gunBarrel = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        gunParticles = gunBarrel.GetComponent<ParticleSystem>();
        gunAudio = gunBarrel.GetComponent<AudioSource>();
        gunLight = gunBarrel.GetComponent<Light>();
        anim = GetComponentInChildren<Animator>();

        gunLines = new List<LineRenderer>();
    }

    void Update()
    {
        if (gunLines.Count == 0) {
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
        for (int i = 0; i < gunLines.Count; i++) {
            gunLines[i].enabled = false;
        }
        gunLight.enabled = false;
    }

    public void CleanGunLine() {
        if (gunLines is not null) {
            for (int i = 0; i < gunLines.Count; i++) {
                gunLines[i].enabled = false;
            }
        }
    }

    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();
        anim.SetTrigger("Shot");

        int bulletsShot = Random.Range(gunLines.Count/2 - 2 * bulletVariation, gunLines.Count/2 - bulletVariation);

        for (int i = 0; i < bulletsShot; i++) {
            gunLines[i].material = new Material(Shader.Find("Sprites/Default"));
            gunLines[i].enabled = true;
            gunLines[i].startColor = Color.red;
            gunLines[i].endColor = Color.green;
            gunLines[i].SetPosition(0, gunBarrel.transform.position);

            shootRay.origin = gunBarrel.transform.position;
            shootRay.direction = (gunBarrel.transform.forward + new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), 0)).normalized;

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
                        if (GameObject.FindGameObjectWithTag("Buff") != null)
                        {
                            GameObject pet = GameObject.FindGameObjectWithTag("Buff");
                            PetBuffAction buff = pet.GetComponent<PetBuffAction>();
                            if (buff.buffed)
                            {
                                    int damageBuff = buff.buffDamage;
                                    int damage = (int)(damageBuff/((shootHit.point - shootRay.origin).magnitude * (shootHit.point - shootRay.origin).magnitude));
                                    enemyHealth.TakeDamage(damage, shootHit.point);
                            }
                            else
                            {
                                int damage = (int)(damagePerShot/((shootHit.point - shootRay.origin).magnitude * (shootHit.point - shootRay.origin).magnitude));
                                enemyHealth.TakeDamage(damage, shootHit.point);
                            }
                        }
                        else
                        {
                            int damage = (int)(damagePerShot/((shootHit.point - shootRay.origin).magnitude * (shootHit.point - shootRay.origin).magnitude));
                            enemyHealth.TakeDamage(damage, shootHit.point);
                        }
                    }

                }

                gunLines[i].SetPosition(1, shootHit.point);
            }
            else
            {
                gunLines[i].SetPosition(1, shootRay.origin + shootRay.direction * range);
                gunLines[i + bulletsShot].enabled = true;
                gunLines[i + bulletsShot].SetPosition(0, shootRay.origin + shootRay.direction * range);
                gunLines[i + bulletsShot].SetPosition(1, shootRay.origin + shootRay.direction * 100f);
                gunLines[i + bulletsShot].startColor = Color.green;
                gunLines[i + bulletsShot].endColor = Color.green;
            }
        }
    }
}
