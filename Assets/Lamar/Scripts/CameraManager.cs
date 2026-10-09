using UnityEngine;

namespace Expelled.Camera
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance { get; private set; }

        [SerializeField] private Transform player;
        [SerializeField] private float smoothSpeed = 5f;

        private Vector3 offset;
        private Vector3 shakeOffset;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            offset = transform.position - player.position;
        }

        public void SetShakeOffset(Vector3 shake)
        {
            shakeOffset = shake;
        }

        void LateUpdate()
        {
            if (player == null) return;

            Vector3 target = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, target, smoothSpeed * Time.deltaTime) + shakeOffset;
            shakeOffset = Vector3.zero;
        }
    }
}
