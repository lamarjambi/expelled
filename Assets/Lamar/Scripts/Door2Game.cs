using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Expelled.Narrative
{
    public class Door2Game : MonoBehaviour
    {
        public PlayableDirector director;

        private void OnEnable()
        {
            StartCoroutine(WaitThenLoad());
        }

        private IEnumerator WaitThenLoad()
        {
            yield return null;
            if (director != null && director.state == PlayState.Playing)
                yield return new WaitWhile(() => director.state == PlayState.Playing);
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
