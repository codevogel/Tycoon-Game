using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Audio
{
    /// <summary>
    /// Singleton behaviour that handles the audio system.
    /// </summary>
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        /// <summary> Determines the volume of everything </summary>
        [Range(0, 1)]
        private float _masterVolume = .5f;
        /// <summary> Determines the volume for SFX type sounds </summary>
        [Range(0, 1)]
        private float _sfxVolume = 1;
        /// <summary> Determines the volume for Music type sounds </summary>
        [Range(0, 1)]
        private float _musicVolume = 1;
        public UnityEvent onVolumeChange = new();

        /// <summary> Determines the volume of everything </summary>
        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                _masterVolume = Mathf.Clamp01(value);
                onVolumeChange.Invoke();
            }
        }

        /// <summary> Determines the volume for SFX type sounds </summary>
        public float SfxVolume
        {
            get => _sfxVolume;
            set
            {
                _sfxVolume = Mathf.Clamp01(value);
                onVolumeChange.Invoke();
            }
        }

        /// <summary> Determines the volume for Music type sounds </summary>
        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = Mathf.Clamp01(value);
                onVolumeChange.Invoke();
            }
        }

        /// <summary> Clears all the listeners</summary>
        private void OnDestroy()
        {
            onVolumeChange.RemoveAllListeners();
        }
    }
}
