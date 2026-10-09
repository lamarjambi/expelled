using UnityEngine;

namespace Expelled.Narrative
{
    public class DoorController : MonoBehaviour
    {
        public static DoorController Instance;
        public AudioSource doorOpenSound;
        private Animator animator;

        void Awake()
        {
            Instance = this;
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            if (PlayerPrefs.GetInt("DoorOpened", 0) == 1)
            {
                if (doorOpenSound != null) doorOpenSound.Play();
                animator.Play("DoorOpen", 0, 1f);
            }
        }

        public void OpenDoor()
        {
            PlayerPrefs.SetInt("DoorOpened", 1);
            PlayerPrefs.Save();
            animator.SetTrigger("Open");
        }
    }
}
