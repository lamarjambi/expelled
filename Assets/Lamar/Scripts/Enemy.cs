using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Expelled.Player;

namespace Expelled.Combat
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float health = 3;
        [SerializeField] GameObject hitVFX;
        [SerializeField] GameObject ragdoll;

        [Header("Combat")]
        [SerializeField] float attackCD = 0.5f; // cooldown 0.5s between attacks
        [SerializeField] float attackRange = 1f;
        [SerializeField] float aggroRange = 4f;

        GameObject player;
        NavMeshAgent agent;
        Animator animator;
        AudioSource hitSound;
        float timePassed;
        float newDestinationCD = 0.5f;

        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            hitSound = GetComponent<AudioSource>();
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);

		    if (player == null)
		    {
                return;
		    }

            if (timePassed >= attackCD)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= attackRange)
                {
                    animator.SetTrigger("Attack");
                    timePassed = 0;
                    StartCoroutine(DamageWindow(0.2f, 0.8f));
                }
            }
            timePassed += Time.deltaTime;

            if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
            {
                newDestinationCD = 0.5f;
                if (agent.isOnNavMesh)
                    agent.SetDestination(player.transform.position);
            }
            newDestinationCD -= Time.deltaTime;
            transform.LookAt(player.transform);
        }

	    private void OnCollisionEnter(Collision collision)
	    {
            if (collision.gameObject.CompareTag("Player"))
            {
                print(true);
                player = collision.gameObject;
            }
        }

	    void Die()
        {
            GameObject r = Instantiate(ragdoll, transform.position, transform.rotation);
            DontDestroyOnLoad(r);
            PlayerState.PersistentRagdolls.Add(r);
            
            string scene = SceneManager.GetActiveScene().name;
            if (scene == "GameScene" || scene == "GameScene2")
            {
                EnemyManager.Instance.OnEnemyDied();
            }
            Destroy(this.gameObject);
        }

        public void TakeDamage(float damageAmount)
        {
            health -= damageAmount;
            if (hitSound != null) hitSound.Play();
            animator.SetTrigger("Damage");
            // CameraShake.Instance.ShakeCamera(2f, 0.2f);

            if (health <= 0)
            {
                Die();
            }
        }

        IEnumerator DamageWindow(float delay, float duration)
        {
            yield return new WaitForSeconds(delay);
            StartDealDamage();
            yield return new WaitForSeconds(duration);
            EndDealDamage();
        }

        public void StartDealDamage()
        {
            GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
        }

        public void EndDealDamage()
        {
            GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
        }

        public void HitVFX(Vector3 hitPosition)
        {
            GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
            Destroy(hit, 3f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
    }
}
