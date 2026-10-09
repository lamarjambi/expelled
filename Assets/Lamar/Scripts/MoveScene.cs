using UnityEngine;
using UnityEngine.SceneManagement;
using Expelled.Player;

namespace Expelled.Narrative
{
    public class MoveScene : MonoBehaviour
    {
        // :desc: this is just the trigger object between gamescene1 and gamescene2
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerState.DoorOpened = true;
                PlayerState.DestroyRagdolls();
                SceneManager.LoadScene("GameScene2");
            }
        }
    }
}
