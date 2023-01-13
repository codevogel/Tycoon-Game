using UnityEngine;

[CreateAssetMenu(fileName = "AudioFile", menuName = "ScriptableObjects/AudioScriptableObject", order = 1)]
public class AudioScriptable : ScriptableObject
{
    public enum AudioType
    {
        SFX,
        Music
    }

    /// <summary> The audio clip that gets played </summary>
    public AudioClip Clip;
    /// <summary> Pitch changes the speed of the audio creating a higher/lower and faster/slower sound </summary>
    [Range(0.01f, 6.0f)]
    public float StartPitch = 1;
    /// <summary> Changes the pitch be a random amount depending on the number given every time 
    /// The pitch can't go below 0.01f though because that would make you unable to hear the audio</summary>
    [Range(0.0f, 3.0f)]
    public float PitchVariation = 0;
    /// <summary> Determines how loud the audio should be played </summary>
    [Range(0, 1)]
    public float Volume = 1;
    /// <summary> Spatial blend causes the audio to be space dependent and softer when </summary>
    [Range(0, 1)]
    public float SpatialBlend = 1;
    /// <summary> Plays the audio again ones its done playing </summary>
    public bool Loop = false;
    /// <summary> Determines which audio sound settings this is effected by </summary>
    public AudioType Type;
    /// <summary> The audiosource that the clip is played from </summary>
    private AudioSource _audioSource;

    public void Play(AudioSource audioSource)
    {
        _audioSource = audioSource;
        //Apply settings and randomize pitch
        _audioSource.clip = Clip;
        _audioSource.pitch = Mathf.Clamp(StartPitch + Random.Range(-PitchVariation, PitchVariation), 0.01f, 10f);
        OnVolumeChange();
        _audioSource.spatialBlend = SpatialBlend;
        _audioSource.loop = Loop;
        //Play audio
        _audioSource.Play();
        //Listen for changes in settings so it updates the volume for this audiosource
        AudioManager.Instance.OnVolumeChange.RemoveListener(OnVolumeChange);
        AudioManager.Instance.OnVolumeChange.AddListener(OnVolumeChange);
    }
    /// <summary>Updates the volume for the audiosource</summary>
    public void OnVolumeChange()
    {
        if (_audioSource)
        {
            float volume = Volume * AudioManager.Instance.MasterVolume;
            switch (Type)
            {
                case AudioType.SFX:
                    volume *= AudioManager.Instance.SFXVolume;
                    break;
                case AudioType.Music:
                    volume *= AudioManager.Instance.MusicVolume;
                    break;
            }
            _audioSource.volume = volume;
        }
    }
}
