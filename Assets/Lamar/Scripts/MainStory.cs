using UnityEngine;
using UnityEngine.SceneManagement;

namespace Expelled.Narrative
{
    public class MainStory : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.LoadScene("Door1Scene", LoadSceneMode.Single);
        }
    }
}
