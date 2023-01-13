using UnityEngine;
using UnityEngine.UI;

public class ChangeAudioSettings : MonoBehaviour
{
    /// <summary> Changes the MasterVolume using a slider </summary>
    /// <param name="masterVolume"></param>
    public void ChangeMasterVolume(Slider masterVolume)
    {
        AudioManager.Instance.MasterVolume = Mathf.Clamp01(masterVolume.value);
    }

    /// <summary>
    /// Changes the SFX volume using a slider
    /// </summary>
    /// <param name="sfxVolume"></param>
    public void ChangeSFXVolume(Slider sfxVolume)
    {
        AudioManager.Instance.SFXVolume = Mathf.Clamp01(sfxVolume.value);
    }

    /// <summary> Changes the Music Volume using a slider </summary>
    /// <param name="musicVolume"></param>
    public void ChangeMusicVolume(Slider musicVolume)
    {
        AudioManager.Instance.MusicVolume = Mathf.Clamp01(musicVolume.value);
    }
}
