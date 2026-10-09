using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Expelled.Player;

namespace Expelled.UI
{
    public class GameWin : MonoBehaviour
    {
        [SerializeField] private string menuSceneName = "MenuScene";
        [SerializeField] private float displayDuration = 5f;

        void Start()
        {
            StartCoroutine(ReturnToMenu());
        }

        private IEnumerator ReturnToMenu()
        {
            yield return new WaitForSeconds(displayDuration);
            PlayerPrefs.DeleteAll();
            PlayerState.Reset();
            Time.timeScale = 1f;
            SceneManager.LoadScene(menuSceneName);
        }
    }
}
