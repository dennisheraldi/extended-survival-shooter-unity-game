using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

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

    /*
    // shotgun
    List<LineRenderer> gunLines;       
    float bulletSpread = 0.2f;     
    int bullets = 5;
    int bulletVariation = 2;    
    float rangeShotgun = 5f;   
    */

    // bow and arrow
    public GameObject arrow;   
    public Slider chargeSlider;     
    int stepCount = 10;
    float angle = Mathf.PI/3;
    float power = 0f;
    float time, v0;
    bool shot = false;

    void Awake()
    {
        shootableMask = LayerMask.GetMask("Shootable");
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();

        /*
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
        */

        /*
        // bow and arrow
        gunLine.material = new Material(Resources.Load("PredictionLine", typeof(Material)) as Material);
        */
    }

    void Update()
    {
        // normal and shotgun
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire... (+ scene manager)
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets && !pauseManager.canvas.enabled)
        {
            Shoot();
        }

        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

        /*
        // bow and arrow
        timer += Time.deltaTime;
        chargeSlider.value = power;
        power = 0f;

        if (Input.GetButtonDown("Fire1"))
        {
            shot = false;
        }

        if (Input.GetButton("Fire1"))
        {
            power = timer <= 2.5f ? timer * 4f : 10f;
            DrawPath(out time, out v0, power, angle, stepCount);
        }

        if (Input.GetButtonUp("Fire1")) {
            StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(time, v0, power));
            shot = true;
        }

        if (shot && timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
        */
    }

    public void DisableEffects()
    {
        // normal
        // Disable the line renderer and the light.
        gunLine.enabled = false;
        //faceLight.enabled = false;
        gunLight.enabled = false;

        /*
        // shotgun
        for (int i = 0; i < gunLines.Count; i++) {
            gunLines[i].enabled = false;
        }
        gunLight.enabled = false;

        /*
        // bow and arrow
        gunLine.enabled = false;
        gunLight.enabled = false;
        */
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
        /*
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
        */
    }

    void DrawPath(out float time, out float v0, float power, float angle, int stepCount) {
        gunLine.enabled = true;
        
        Vector3 direction = transform.forward;

        float xt = power;
        float yt = -0.3f;

        float num = Mathf.Pow(xt, 2) * -Physics.gravity.y;
        float denom = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle) - 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(num/denom);

        time = xt / (v0 * Mathf.Cos(angle));
        float timeStep = time/stepCount;

        Vector3[] positions = new Vector3[stepCount + 1];
        for (int i = 0; i <= stepCount; i++) {
            float t = i * timeStep;
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * t * t;
            positions[i] = transform.position + direction * x + Vector3.up * y;
        }
        gunLine.positionCount = stepCount + 1;
        gunLine.SetPositions(positions);
    }

    IEnumerator Coroutine_Movement(float time, float v0, float power) {
        // bow and arrow
        timer = 0f;

        gunAudio.Play();

        gunLight.enabled = true;

        gunParticles.Stop();
        gunParticles.Play();

        Vector3 startPosition = transform.position;
        Vector3 direction = transform.forward;
        GameObject arrowGameObject = Instantiate(arrow, transform.position, transform.rotation) as GameObject;
        Rigidbody arrowRigidbody = arrowGameObject.GetComponent<Rigidbody>();

        float t = 0f;
        float vx, vy, x, y;
        while (t < time) {
            x = v0 * t * Mathf.Cos(angle);
            y = v0 * t * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * t * t;
            vx = v0 * Mathf.Cos(angle);
            vy = v0 * Mathf.Sin(angle) - -Physics.gravity.y * t;
            float angleArrow = Mathf.Atan2(vy, vx) * Mathf.Rad2Deg;
            arrowRigidbody.MoveRotation(transform.rotation * Quaternion.Euler(Vector3.right * -angleArrow));
            arrowRigidbody.MovePosition(startPosition + direction * x + Vector3.up * y);
            t += Time.deltaTime;
            yield return null;
        }

        gunLine.enabled = true;
    } 
}