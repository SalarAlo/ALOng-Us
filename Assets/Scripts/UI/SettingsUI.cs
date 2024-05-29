using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : BaseUISingletonPersistent<SettingsUI>
{
    public Action<float> OnOverallVolumeSliderChanged;
    public Action<float> OnMusicVolumeSliderChanged;
    public Action<float> OnSoundVolumeSliderChanged;

    [SerializeField] private Button closeSettingsButton;
    [SerializeField] private Slider overallVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundsVolumeSlider;

    private void Start() {
        closeSettingsButton.onClick.AddListener(CloseSettingsButton_OnClick);
        overallVolumeSlider.onValueChanged.AddListener(OverallVolumeSlider_OnValueChanged);
        musicVolumeSlider.onValueChanged.AddListener(MusicVolumeSlider_OnValueChanged);
        soundsVolumeSlider.onValueChanged.AddListener(SoundVolumeSlider_OnValueChanged);
        Hide();
    }

    private void OverallVolumeSlider_OnValueChanged(float newVolume) {
        OnOverallVolumeSliderChanged?.Invoke(newVolume);
    }
    
    private void MusicVolumeSlider_OnValueChanged(float newVolume) {
        OnMusicVolumeSliderChanged?.Invoke(newVolume);
    }
    private void SoundVolumeSlider_OnValueChanged(float newVolume) {
        OnSoundVolumeSliderChanged?.Invoke(newVolume);
    }

    private void CloseSettingsButton_OnClick(){
        Hide();
    }
}
