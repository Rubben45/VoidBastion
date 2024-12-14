using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer mixer;
    
    private bool ignoreFirstCall = true;
    private bool ignoreFirstCall_2 = true;

    private void Start()
    {
        LoadAudio();
    }

    private void LoadAudio()
    {
        sfxSlider.value = 1f;
        musicSlider.value = 1f;

        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }
    
    private void OnSFXVolumeChanged(float SFXvalue)
    {
        if (ignoreFirstCall)
        {
            ignoreFirstCall = false;
            return;
        }

        SetSFXVolume(SFXvalue);
    }

    private void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    private void OnMusicVolumeChanged(float musicValue)
    {
        if (ignoreFirstCall_2)
        {
            ignoreFirstCall_2 = false;
            return;
        }

        SetMusicVolume(musicValue);
    }

    private void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }
}
