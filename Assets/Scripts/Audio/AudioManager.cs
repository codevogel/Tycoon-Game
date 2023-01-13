using UnityEngine;
using UnityEngine.Events;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    /// <summary> Determines the volume of everything </summary>
    [Range(0, 1)]
    private float _masterVolume = .5f;
    /// <summary> Determines the volume for SFX type sounds </summary>
    [Range(0, 1)]
    private float _SFXVolume = 1;
    /// <summary> Determines the volume for Music type sounds </summary>
    [Range(0, 1)]
    private float _musicVolume = 1;
    public UnityEvent OnVolumeChange = new UnityEvent();

    /// <summary> Determines the volume of everything </summary>
    public float MasterVolume
    {
        get { return _masterVolume; }
        set
        {
            _masterVolume = Mathf.Clamp01(value);
            OnVolumeChange.Invoke();
        }
    }

    /// <summary> Determines the volume for SFX type sounds </summary>
    public float SFXVolume
    {
        get { return _SFXVolume; }
        set
        {
            _SFXVolume = Mathf.Clamp01(value);
            OnVolumeChange.Invoke();
        }
    }

    /// <summary> Determines the volume for Music type sounds </summary>
    public float MusicVolume
    {
        get { return _musicVolume; }
        set
        {
            _musicVolume = Mathf.Clamp01(value);
            OnVolumeChange.Invoke();
        }
    }

    /// <summary> Clears all the listeners</summary>
    private void OnDestroy()
    {
        OnVolumeChange.RemoveAllListeners();
    }
}
