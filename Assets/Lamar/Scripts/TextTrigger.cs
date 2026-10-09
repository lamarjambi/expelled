using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Expelled.Player;
using Expelled.Combat;

namespace Expelled.Narrative
{
    public class TextTrigger : MonoBehaviour
    {
        [SerializeField] GameObject goalText;
        [SerializeField] GameObject[] enemies;
        [SerializeField] float displayDuration = 3f;
        [SerializeField] float dropHeight = 8f;
        [SerializeField] float dropDuration = 1f;
        [SerializeField] float spawnStagger = 0.15f;

        void Start()
        {
            // :desc: hide goal text and all enemies until the trigger fires
            if (goalText != null)
                goalText.SetActive(false);

            foreach (var enemy in enemies)
                enemy.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            // :desc: fire the sequence once when the player enters, then disable the collider
            if (!other.CompareTag("Player")) return;
            if (PlayerState.TextTriggered) return;

            PlayerState.TextTriggered = true;
            StartCoroutine(TriggerSequence());
            GetComponent<Collider>().enabled = false;
        }

        IEnumerator TriggerSequence()
        {
            // :desc: show goal text, wait, then drop enemies in one by one with stagger
            if (goalText != null)
            {
                goalText.SetActive(true);
                yield return new WaitForSeconds(displayDuration);
                goalText.SetActive(false);
            }

            yield return new WaitForSeconds(3f);

            foreach (var enemy in enemies)
            {
                StartCoroutine(DropEnemy(enemy));
                yield return new WaitForSeconds(spawnStagger);
            }

            EnemyManager.Instance.InitializeCount();
        }

        IEnumerator DropEnemy(GameObject enemy)
        {
            // :desc: animate enemy falling from above to its spawn position
            // :param enemy: the enemy gameobject to drop
            Vector3 targetPos = enemy.transform.position;
            enemy.transform.position = targetPos + Vector3.up * dropHeight;
            enemy.SetActive(true);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null) agent.enabled = false;

            float elapsed = 0f;
            while (elapsed < dropDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / dropDuration;
                // t*t gives ease-in, mimicking gravity acceleration
                enemy.transform.position = Vector3.Lerp(targetPos + Vector3.up * dropHeight, targetPos, t * t);
                yield return null;
            }

            enemy.transform.position = targetPos;
            if (agent != null) agent.enabled = true;
        }
    }
}
