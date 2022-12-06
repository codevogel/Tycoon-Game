using UnityEngine;
using UnityEngine.UI;

public class ChangeAudioSettings : MonoBehaviour
{
    public void ChangeMasterVolume(Slider masterVolume)
    {
        AudioManager.Instance.MasterVolume = Mathf.Clamp01(masterVolume.value);
    }

    public void ChangeSFXVolume(Slider sfxVolume)
    {
        AudioManager.Instance.SFXVolume = Mathf.Clamp01(sfxVolume.value);
    }

    public void ChangeMusicVolume(Slider musicVolume)
    {
        AudioManager.Instance.MusicVolume = Mathf.Clamp01(musicVolume.value);
    }
}
