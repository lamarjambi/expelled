using UnityEngine;

namespace Expelled.Player
{
    public class HealingArea : MonoBehaviour
    {
        [SerializeField] float healPerSecond = 1f;
        [SerializeField] GameObject healVFX;
        [SerializeField] float vfxHeightOffset = 1f;

        float healTimer;
        HealthSystem playerHealth;
        Transform playerTransform;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerHealth = other.GetComponent<HealthSystem>();
                playerTransform = other.transform;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerHealth = null;
                playerTransform = null;
            }
        }

        void Update()
        {
            if (playerHealth == null) return;

            healTimer += Time.deltaTime;
            if (healTimer >= 1f)
            {
                playerHealth.Heal(healPerSecond);
                healTimer = 0f;

                if (healVFX != null)
                {
                    Vector3 spawnPos = playerTransform.position + Vector3.up * vfxHeightOffset;
                    GameObject vfx = Instantiate(healVFX, spawnPos, Quaternion.identity);
                    Destroy(vfx, 3f);
                }
            }
        }
    }
}
