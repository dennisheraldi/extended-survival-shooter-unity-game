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
    GameObject gun;

    // scene manager
    // public PauseManager pauseManager;

    void Start()
    {
        shootableMask = LayerMask.GetMask("Shootable");

        gunBarrel = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        gun = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        gunParticles = gunBarrel.GetComponent<ParticleSystem>();
        gunLine = gunBarrel.GetComponent<LineRenderer>();
        gunAudio = gunBarrel.GetComponent<AudioSource>();
        gunLight = gunBarrel.GetComponent<Light>();
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire... (+ scene manager)
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

    public void CleanGunLine() {
        gunLine.positionCount = 0;
    }

    void Shoot()
    {
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;
        //faceLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        gunLine.material = new Material(Shader.Find("Sprites/Default"));
        gunLine.enabled = true;
        gunLine.startColor = Color.yellow;
        gunLine.endColor = Color.yellow;
        gunLine.positionCount = 2;
        gunLine.SetPosition(0, gunBarrel.transform.position);

        shootRay.origin = gunBarrel.transform.position;
        shootRay.direction = gunBarrel.transform.forward;

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
                    if (GameObject.FindGameObjectWithTag("Buff") != null)
                    {
                        GameObject pet = GameObject.FindGameObjectWithTag("Buff");
                        PetBuffAction buff = pet.GetComponent<PetBuffAction>();
                        if (buff.buffed)
                        {
                                int damageBuff = buff.buffDamage;
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
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    IEnumerator Animate() {
        Vector3 position = gun.transform.localPosition;
        Vector3 direction = gunBarrel.transform.forward;
        float x = 0f;

        while (x > -0.3f) {
            gun.transform.localPosition = new Vector3(gun.transform.localPosition.x + x, gun.transform.localPosition.y, gun.transform.localPosition.z);
            x -= 0.15f;
            yield return new WaitForSeconds(0.02f);
        }
        while (x < 0f) {
            gun.transform.localPosition = new Vector3(gun.transform.localPosition.x - x, gun.transform.localPosition.y, gun.transform.localPosition.z);
            x += 0.15f;
            yield return new WaitForSeconds(0.02f);
        }
        gun.transform.localPosition = new Vector3(0, 0, 0);
    }
}
