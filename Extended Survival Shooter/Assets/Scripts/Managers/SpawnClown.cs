using UnityEngine;

public class SpawnClown : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    public Animator animator;

    private bool hasSpawned;

    void Start ()
    {
        hasSpawned = false;
    }

    void Update()
    {
        // if animator is destroyed, stop spawning
        if (animator == null)
        {
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Angry") && !hasSpawned)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.51)
            {
                Spawn();
                hasSpawned = true;
            }

        }
        else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Angry") && hasSpawned)
        {
            hasSpawned = false;
        }
    }

    void Spawn ()
    {
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}