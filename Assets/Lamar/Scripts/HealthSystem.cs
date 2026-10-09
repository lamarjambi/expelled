using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Expelled.Camera;

namespace Expelled.Player
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] float maxHealth = 10;
        [SerializeField] RectTransform healthBarFill;
        [SerializeField] GameObject hitVFX;
        [SerializeField] GameObject ragdoll;
        [SerializeField] AudioSource hitSound;

        float health;
        bool isDead;
        Animator animator;

        void Start()
        {
            // :desc: load persisted health or default to max, then sync the health bar
            animator = GetComponent<Animator>();
            health = PlayerPrefs.GetFloat("PlayerHealth", maxHealth);
            //health = maxHealth;
            UpdateHealthBar();
        }

        public float GetHealth() => health;

        public void TakeDamage(float damageAmount)
        {
            // :desc: apply damage, trigger hit animation and camera shake
            // :desc: call die if health hits zero
            // :param damageAmount: how much health to subtract
            if (hitSound != null) hitSound.Play();
            if (isDead) return;

            health = Mathf.Max(health - damageAmount, 0);
            UpdateHealthBar();
            animator.SetTrigger("Damage");
            CameraShake.Instance.ShakeCamera(1f, 0.2f);

            if (health <= 0)
            {
                Die();
            }
        }

        void UpdateHealthBar()
        {
            // :desc: scale health bar fill to match current health ratio
            if (healthBarFill != null)
                healthBarFill.anchorMax = new Vector2(health / maxHealth, 1);
        }

        void OnDestroy()
        {
            // :desc: persist health to PlayerPrefs on destroy, only if still alive
            if (health > 0)
            {
                PlayerPrefs.SetFloat("PlayerHealth", health);
                PlayerPrefs.Save();
            }
        }

        void Die()
        {
            // :desc: to make sure that the ragdoll doesnt keep instantiating everytime player is hit
            isDead = true;
            
            foreach (var c in GetComponentsInChildren<Collider>())
                c.enabled = false;
            Instantiate(ragdoll, transform.position, transform.rotation);
            foreach (var r in GetComponentsInChildren<Renderer>())
                r.enabled = false;

            StartCoroutine(LoadGameOver());
        }

        IEnumerator LoadGameOver()
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("GameOver");
        }

        public void Heal(float amount)
        {
            // :desc: restore health up to max and update the bar
            // :param amount: how much health to restore
            health = Mathf.Min(health + amount, maxHealth);
            UpdateHealthBar();
        }

        public void HitVFX(Vector3 hitPosition)
        {
            // :desc: spawn hit vfx at the impact point and auto-destroy it
            // :param hitPosition: world position where the hit landed
            GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
            Destroy(hit, 3f);
        }
    }
}
