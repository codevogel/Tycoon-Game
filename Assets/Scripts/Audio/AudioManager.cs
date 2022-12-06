using UnityEngine;
using UnityEngine.Events;

public class AudioManager : SingletonBehaviour<AudioManager>
{
    [Range(0, 1)]
    private float _masterVolume = .5f;
    [Range(0, 1)]
    private float _SFXVolume = 1;
    [Range(0, 1)]
    private float _musicVolume = 1;
    public UnityEvent OnVolumeChange = new UnityEvent();

    public float MasterVolume
    {
        get { return _masterVolume; }
        set
        {
            _masterVolume = Mathf.Clamp01(value);
            OnVolumeChange.Invoke();
        }
    }

    public float SFXVolume
    {
        get { return _SFXVolume; }
        set
        {
            _SFXVolume = Mathf.Clamp01(value);
            OnVolumeChange.Invoke();
        }
    }

    public float MusicVolume
    {
        get { return _musicVolume; }
        set
        {
            _musicVolume = Mathf.Clamp01(value);
            OnVolumeChange.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnVolumeChange.RemoveAllListeners();
    }
}
