using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSoundManager : SingletonPersistent<LocalSoundManager>
{
    [SerializeField] private SingleAudioSource audioSourcePrefab;
    [SerializeField] private AudioSource musicManager;
    [SerializeField] private float overallVolumeSettings = 1;
    [SerializeField] private float musicVolumeSettings = 1;
    [SerializeField] private float soundsVolumeSettings = 1;

    private float originalMusicVolume;

    public override void Awake()
    {
        base.Awake();
        originalMusicVolume = musicManager.volume;
    }

    private void Start() {
        SettingsUI.Instance.OnOverallVolumeSliderChanged += SettingsUI_OnOverallVolumeSliderChanged;
        SettingsUI.Instance.OnMusicVolumeSliderChanged += SettingsUI_OnMusicVolumeSliderChanged;
        SettingsUI.Instance.OnSoundVolumeSliderChanged += SettingsUI_OnSoundVolumeSliderChanged;
        DontDestroyOnLoad(musicManager);
    }

    private void SettingsUI_OnOverallVolumeSliderChanged(float newPercentage) {
        overallVolumeSettings = newPercentage;
        musicManager.volume = originalMusicVolume*newPercentage*musicVolumeSettings;
    }

    private void SettingsUI_OnMusicVolumeSliderChanged(float newPercentage){
        musicVolumeSettings = newPercentage;
        musicManager.volume = originalMusicVolume*newPercentage*overallVolumeSettings;
    }
    
    private void SettingsUI_OnSoundVolumeSliderChanged(float newPercentage){
        soundsVolumeSettings = newPercentage;
    }

    public void PlaySound(AudioClip clip){
        SingleAudioSource singleAudioSource = Instantiate(audioSourcePrefab, transform);
        singleAudioSource.GetAudioSource().volume *= overallVolumeSettings * soundsVolumeSettings;
        singleAudioSource.Play(clip);
    }
}
