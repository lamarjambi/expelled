using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Expelled.UI
{
    public class Settings : MonoBehaviour
    {
        public Slider masterVol;
        public AudioMixer mainAudioMixer;

        void Start()
        {
            float saved = PlayerPrefs.GetFloat("MasterVol", 1f);
            masterVol.value = saved;
            ApplyVolume(saved);
        }

        public void ChangeMasterVolume()
        {
            float val = masterVol.value;
            PlayerPrefs.SetFloat("MasterVol", val);
            ApplyVolume(val);
        }

        void ApplyVolume(float linearVal)
        {
            AudioListener.volume = linearVal;
            float dB = linearVal > 0.0001f ? Mathf.Log10(linearVal) * 20f : -80f;
            mainAudioMixer.SetFloat("MasterVol", dB);
        }
    }
}
