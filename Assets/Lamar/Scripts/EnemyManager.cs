using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Expelled.Narrative;
using Expelled.Player;

namespace Expelled.Combat
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance;
        private int enemyCount;

        void Awake()
        {
            Instance = this;
            InitializeCount();
        }

        public void InitializeCount()
        {
            // :desc: count all active enemies by tag
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }

        public void OnEnemyDied()
        {
            // :desc: decrement count + trigger scene transition when all enemies are dead
            enemyCount--;
            string scene = SceneManager.GetActiveScene().name;
            if (enemyCount <= 0 && scene == "GameScene")
                StartCoroutine(LoadDoorScene());
            else if (enemyCount <= 0 && scene == "GameScene2")
                StartCoroutine(LoadGameWin());
        }

        private IEnumerator LoadGameWin()
        {
            // :desc: wait then load win screen
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("GameWin");
        }

        private IEnumerator LoadDoorScene()
        {
            // :desc: open door, save player position, then load the door scene
            if (DoorController.Instance != null)
                DoorController.Instance.OpenDoor();

            yield return new WaitForSeconds(2f);

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerState.Position = player.transform.position;
                PlayerState.HasData = true;
            }

            SceneManager.LoadScene("Door2Scene", LoadSceneMode.Single);
        }
    }
}
