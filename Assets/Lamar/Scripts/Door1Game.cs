using UnityEngine;
using UnityEngine.SceneManagement;

namespace Expelled.Narrative
{
    public class Door1Game : MonoBehaviour
    {
        private void OnEnable()
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
