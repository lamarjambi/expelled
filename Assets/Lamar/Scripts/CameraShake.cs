using System.Collections;
using UnityEngine;

namespace Expelled.Camera
{
    public class CameraShake : MonoBehaviour
    {
        // singleton
        public static CameraShake Instance { get; private set; }
        void Awake()
        {
            Instance = this;
        }

        public void ShakeCamera(float intensity, float time)
        {
            // :desc: start a camera shake
            // :param intensity: how strong the shake is
            // :param time: how long the shake lasts
            StartCoroutine(Shake(intensity, time));
        }

        private IEnumerator Shake(float intensity, float time)
        {
            // :desc: coroutine that offsets camera position randomly
            // :param intensity: max displacement amount
            // :param time: total shake duration
            float elapsed = 0f;

            while (elapsed < time)
            {
                float strength = Mathf.Lerp(intensity, 0f, elapsed / time);
                CameraManager.Instance.SetShakeOffset(Random.insideUnitSphere * strength);
                elapsed += Time.deltaTime;
                yield return null;
            }

            CameraManager.Instance.SetShakeOffset(Vector3.zero);
        }
    }
}
