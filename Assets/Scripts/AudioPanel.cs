using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{
    private const string MasterVolume = "MasterVolume";
    private const string ButtonVolume = "ButtonVolume";
    private const string FonVolume = "FonVolume";
    private const string IsMuted = "IsMuted";
    private const float VolumeMultiple = 20f;

    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private List<SliderUi> _slidersUi;
    [SerializeField] private Toggle _toggleButton;

    private bool _isMuted;
    private float _minDCBValue = -80f;

    private void OnEnable()
    {
        foreach (var slider in _slidersUi)
            slider.VolumeChanged += ChangerVolume;

        _toggleButton.onValueChanged.AddListener(HandleMuteToggle);
    }

    private void Awake()
    {
        LoadVolumes();
    }

    private void OnDisable()
    {
        foreach (var slider in _slidersUi)
            slider.VolumeChanged -= ChangerVolume;

        _toggleButton.onValueChanged.RemoveListener(HandleMuteToggle);
        SaveVolumes();
    }

    private void SaveVolumes()
    {
        foreach (var slider in _slidersUi)
            PlayerPrefs.SetFloat(slider.Key, slider.Value);

        PlayerPrefs.SetInt(IsMuted, _isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadVolumes()
    {
        float startvalue = 1f;

        foreach (var slider in _slidersUi)
        {
            float savedValue = PlayerPrefs.GetFloat(slider.Key, startvalue);
            slider.SetValue(savedValue);
            _audioMixerGroup.audioMixer.SetFloat(slider.Key, SetFloatVolume(savedValue));
        }

        _isMuted = PlayerPrefs.GetInt(IsMuted, 0) == 1;
        _toggleButton.isOn = _isMuted;

        if (_isMuted)
            _audioMixerGroup.audioMixer.SetFloat(MasterVolume, _minDCBValue);
    }

    private void ApplyAllVolumes()
    {
        foreach (var slider in _slidersUi)
        {
            string key = GetEnumParameter(slider.Type);
            float value = slider.Value;

            if (_isMuted)
            {
                _audioMixerGroup.audioMixer.SetFloat(key, _minDCBValue);
            }
            else
            {
                _audioMixerGroup.audioMixer.SetFloat(key, SetFloatVolume(value));
            }
        }
    }

    private void HandleMuteToggle(bool muted)
    {
        _isMuted = muted;
        ApplyAllVolumes();
        SaveVolumes();
    }

    private void ChangerVolume(VolumeType type, float volume)
    {
        PlayerPrefs.SetFloat(GetEnumParameter(type), volume);

        if (!_isMuted)
            _audioMixerGroup.audioMixer.SetFloat(GetEnumParameter(type), SetFloatVolume(volume));
    }

    private string GetEnumParameter(VolumeType type)
    {
        switch (type)
        {
            case VolumeType.MasterVolume: return MasterVolume;
            case VolumeType.ButtonVolume: return ButtonVolume;
            case VolumeType.FonVolume: return FonVolume;
            default: return MasterVolume;
        }
    }

    private float SetFloatVolume(float value)
    {
        value = Mathf.Clamp(value, 0.0001f, 1f);
        return Mathf.Log10(value) * VolumeMultiple;
    }
}
