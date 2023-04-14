using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bow : MonoBehaviour
{
    public int damagePerShot = 20;              
    public float timeBetweenBullets = 0.15f;       

    float timer;                                                         
    ParticleSystem gunParticles;  
    LineRenderer gunLine;                                
    AudioSource gunAudio;                                                     
    float effectsDisplayTime = 0.2f;

    GameObject gunBarrel;

    public GameObject arrow;   
    public Slider chargeSlider;     
    int stepCount = 10;
    float angle = Mathf.PI/3;
    float power = 0f;
    float time, v0;
    bool shot = false;

    // scene manager
    // public PauseManager pauseManager;

    void Start()
    {
        gunBarrel = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;
        gunParticles = gunBarrel.GetComponent<ParticleSystem>();
        gunLine = gunBarrel.GetComponent<LineRenderer>();
        gunAudio = gunBarrel.GetComponent<AudioSource>();

        gunLine.material = new Material(Resources.Load("PredictionLine", typeof(Material)) as Material);
        gunLine.enabled = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        chargeSlider.value = power;
        power = 0f;

        if (Input.GetButtonDown("Fire1"))
        {
            shot = false;
            timer = 0f;
        }

        if (Input.GetButton("Fire1"))
        {
            power = timer <= 2.5f ? timer * 4f : 10f;
            DrawPath(out time, out v0, power, angle, stepCount);
        }

        if (Input.GetButtonUp("Fire1")) {
            // StopAllCoroutines();
            StartCoroutine(Coroutine_Movement(time, v0, power));
            shot = true;
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
        gunLine.enabled = true;
        
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

    IEnumerator Coroutine_Movement(float time, float v0, float power) {
        // bow and arrow
        timer = 0f;

        gunAudio.Play();

        gunParticles.Stop();
        gunParticles.Play();

        Vector3 startPosition = gunBarrel.transform.position;
        Vector3 direction = gunBarrel.transform.forward;
        GameObject arrowGameObject = Instantiate(arrow, gunBarrel.transform.position, gunBarrel.transform.rotation) as GameObject;
        arrowGameObject.GetComponent<Arrow>().time = time;
        Rigidbody arrowRigidbody = arrowGameObject.GetComponent<Rigidbody>();

        float t = 0f;
        float vx, vy, x, y;
        while (t < time) {
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
