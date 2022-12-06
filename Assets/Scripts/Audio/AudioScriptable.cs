using UnityEngine;

[CreateAssetMenu(fileName = "AudioFile", menuName = "ScriptableObjects/AudioScriptableObject", order = 1)]
public class AudioScriptable : ScriptableObject
{
    public enum AudioType
    {
        SFX,
        Music
    }


    protected static AudioSource DefaultAudioSource;
    public AudioClip Clip;
    [Range(-3.0f, 3.0f)]
    public float StartPitch = 1;
    [Range(0.0f, 3.0f)]
    public float PitchVariation = 0;
    [Range(0, 1)]
    public float Volume = 1;
    [Range(0, 1)]
    public float SpatialBlend = 1;
    public bool Loop = false;
    public AudioType Type;
    private AudioSource _audioSource;

    //Can be used but not recommended
    public void Play()
    {
        if (!_audioSource)
        {
            if (!DefaultAudioSource)
            {
                GameObject defaultAudioSource = new GameObject();
                defaultAudioSource.name = "DefaultAudioSource";
                DontDestroyOnLoad(defaultAudioSource);
                DefaultAudioSource = defaultAudioSource.AddComponent<AudioSource>();
                DefaultAudioSource.playOnAwake = false;
            }
        }
        Play(DefaultAudioSource);
    }

    //Use this
    public void Play(AudioSource audioSource)
    {
        _audioSource = audioSource;

        _audioSource.clip = Clip;
        _audioSource.pitch = StartPitch + Random.Range(-PitchVariation, PitchVariation);
        OnVolumeChange();
        _audioSource.spatialBlend = SpatialBlend;
        _audioSource.loop = Loop;
        _audioSource.Play();

        AudioManager.Instance.OnVolumeChange.RemoveListener(OnVolumeChange);
        AudioManager.Instance.OnVolumeChange.AddListener(OnVolumeChange);
    }

    //Probably should never be used because getcomponent is slow
    public void PlayOnThisObject(GameObject gameObject)
    {
        if (!gameObject.TryGetComponent(out _audioSource))
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        Play(_audioSource);
    }
    
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
