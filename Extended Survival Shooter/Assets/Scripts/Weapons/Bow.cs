using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{
    public int damagePerShot = 100;              
    public float timeBetweenBullets = 1f;       

    float timer = 1f;
    float timerPower;                                                         
    ParticleSystem gunParticles;  
    LineRenderer gunLine;                                
    AudioSource gunAudio;                                                     
    float effectsDisplayTime = 0.2f;

    Animator anim;

    GameObject gunBarrel;

    public GameObject arrow;   
    public Slider chargeSlider;     
    int stepCount = 10;
    float angle = Mathf.PI/3;
    float power = 0f;
    float time, v0;
    bool shot = false;
    bool validClick = false;
    public bool switchWeaponAble = true;

    // scene manager
    // public PauseManager pauseManager;

    void Start()
    {
        gunBarrel = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        gunParticles = gunBarrel.GetComponent<ParticleSystem>();
        gunLine = gunBarrel.GetComponent<LineRenderer>();
        gunAudio = gunBarrel.GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();

        gunLine.material = new Material(Resources.Load("PredictionLine", typeof(Material)) as Material);
    }

    void Update()
    {
        timer += Time.deltaTime;
        timerPower += Time.deltaTime;
        chargeSlider.value = power;
        power = 0f;

        if (timer >= time && timer > 1f) {
            switchWeaponAble = true;
        }

        if (Input.GetButtonDown("Fire2") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            shot = false;
            timerPower = 0f;
            validClick = true;
        }

        if (Input.GetButton("Fire2") && validClick && Time.timeScale != 0)
        {
            
            power = timerPower <= 2.5f ? timerPower * 4f : 10f;
            if (power != 0) DrawPath(out time, out v0, power, angle, stepCount);
        }

        if (Input.GetButtonUp("Fire2") && validClick && Time.timeScale != 0) {
            // StopAllCoroutines();
            timer = 0f;
            CleanGunLine();
            StartCoroutine(Coroutine_Movement(time, v0, power));
            validClick = false;
            shot = true;
            switchWeaponAble = false;
        }

        if (shot && timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        gunLine.enabled = false;
    }

    void DrawPath(out float time, out float v0, float power, float angle, int stepCount) {
        gunLine.material = new Material(Resources.Load("PredictionLine", typeof(Material)) as Material);
        gunLine.enabled = true;
        gunLine.startColor = Color.white;
        gunLine.endColor = Color.white;
        
        Vector3 direction = gunBarrel.transform.forward;

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
            positions[i] = gunBarrel.transform.position + direction * x + Vector3.up * y;
        }
        gunLine.positionCount = stepCount + 1;
        gunLine.SetPositions(positions);
    }

    public void CleanGunLine() {
        if (gunLine) gunLine.positionCount = 0;
    }

    IEnumerator Coroutine_Movement(float time, float v0, float power) {
        // bow and arrow
        gunAudio.Play();

        gunParticles.Stop();
        gunParticles.Play();
        anim.SetTrigger("Flip");

        Vector3 startPosition = gunBarrel.transform.position;
        Vector3 direction = gunBarrel.transform.forward;
        GameObject arrowGameObject = Instantiate(arrow, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
        arrowGameObject.GetComponent<Arrow>().time = time;
        Rigidbody arrowRigidbody = arrowGameObject.GetComponent<Rigidbody>();

        float t = 0f;
        float vx, vy, x, y;
        while (t < time && arrowRigidbody) {
            x = v0 * t * Mathf.Cos(angle);
            y = v0 * t * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * t * t;
            vx = v0 * Mathf.Cos(angle);
            vy = v0 * Mathf.Sin(angle) - -Physics.gravity.y * t;
            float angleArrow = Mathf.Atan2(vy, vx) * Mathf.Rad2Deg;
            arrowRigidbody.MoveRotation(gunBarrel.transform.rotation * Quaternion.Euler(Vector3.right * -angleArrow));
            arrowRigidbody.MovePosition(startPosition + direction * x + Vector3.up * y);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
