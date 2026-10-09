using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Expelled.UI
{
    public class Back2Menu : MonoBehaviour
    {
        public AudioSource audio;

        public void OnClickEvent()
        {
            StartCoroutine(PlayThenLoad());
        }

        private System.Collections.IEnumerator PlayThenLoad()
        {
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);
            SceneManager.LoadScene("MenuScene");
        }
    }
}
