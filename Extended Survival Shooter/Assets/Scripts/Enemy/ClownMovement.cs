using UnityEngine;

public class ClownMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
        PlayerHealth playerHealth;      // Reference to the player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.
        Animator animator;
        AudioSource enemyAudio;
        public float rageInterval = 10f; // time interval for entering rage state
        

        public AudioClip deathClip;
        private AudioClip originalClip;                  // The sound to play when the enemy dies.

        private float nextRageTime;

        void Awake()
        {
            nextRageTime = Time.time + rageInterval;
            enemyAudio = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            originalClip = enemyAudio.clip;
            // Set up the references.
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerHealth = player.GetComponent<PlayerHealth>();
            enemyHealth = GetComponent<EnemyHealth>();
            nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }


        void Update()
        {
            if (Time.time >= nextRageTime)
            {
                animator.SetBool("isRaging", true); // set the isRageActive parameter to true
                nextRageTime = Time.time + rageInterval; // set time for next rage state
                enemyAudio.clip = deathClip;
                enemyAudio.Play();
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Angry"))
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75)
                {
                    animator.SetBool("isRaging", false); // set the isRageActive parameter to false
                    enemyAudio.clip = originalClip;
                }
            }
            // If the enemy and the player have health left...
            if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && !animator.GetBool("isRaging"))
            {
                nav.enabled = true;
                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination(player.position);
            }

            
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                nav.enabled = false;
            }
        }
    }