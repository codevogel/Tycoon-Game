using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(fileName = "AudioFile", menuName = "ScriptableObjects/AudioScriptableObject", order = 1)]
    public class AudioScriptable : ScriptableObject
    {
        public enum AudioType
        {
            SFX,
            Music
        }

        /// <summary> The audio clip that gets played </summary>
        public AudioClip clip;
        /// <summary> Pitch changes the speed of the audio creating a higher/lower and faster/slower sound </summary>
        [Range(0.01f, 6.0f)]
        public float startPitch = 1;
        /// <summary> Changes the pitch be a random amount depending on the number given every time 
        /// The pitch can't go below 0.01f though because that would make you unable to hear the audio</summary>
        [Range(0.0f, 3.0f)]
        public float pitchVariation = 0;
        /// <summary> Determines how loud the audio should be played </summary>
        [Range(0, 1)]
        public float volume = 1;
        /// <summary> Spatial blend causes the audio to be space dependent and softer when </summary>
        [Range(0, 1)]
        public float spatialBlend = 1;
        /// <summary> Plays the audio again ones its done playing </summary>
        public bool loop = false;
        /// <summary> Determines which audio sound settings this is effected by </summary>
        public AudioType type;
        /// <summary> The audiosource that the clip is played from </summary>
        private AudioSource _audioSource;

        public void Play(AudioSource audioSource)
        {
            _audioSource = audioSource;
            //Apply settings and randomize pitch
            _audioSource.clip = clip;
            _audioSource.pitch = Mathf.Clamp(startPitch + Random.Range(-pitchVariation, pitchVariation), 0.01f, 10f);
            OnVolumeChange();
            _audioSource.spatialBlend = spatialBlend;
            _audioSource.loop = loop;
            //Play audio
            _audioSource.Play();
            //Listen for changes in settings so it updates the volume for this audiosource
            AudioManager.Instance.onVolumeChange.RemoveListener(OnVolumeChange);
            AudioManager.Instance.onVolumeChange.AddListener(OnVolumeChange);
        }
        /// <summary>Updates the volume for the audiosource</summary>
        public void OnVolumeChange()
        {
            if (!_audioSource) return;
            float localVolume = volume * AudioManager.Instance.MasterVolume;
            switch (type)
            {
                case AudioType.SFX:
                    localVolume *= AudioManager.Instance.SfxVolume;
                    break;
                case AudioType.Music:
                    localVolume *= AudioManager.Instance.MusicVolume;
                    break;
            }
            _audioSource.volume = localVolume;
        }
    }
}
