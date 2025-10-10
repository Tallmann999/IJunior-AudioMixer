using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioPanel : MonoBehaviour
{
    private const string MasterVolume = "MasterVolume";
    private const string ButtonVolume = "ButtonVolume";
    private const string FonVolume = "FonVolume";
    private const float VolumeMultiple = 20f;

    [SerializeField] private AudioMixerGroup _audioMixerGroup;
    [SerializeField] private List<Slider> _sliders;

    private bool _isMasterToogleOn;
    private float _maxDCBValue = 0f;
    private float _minDCBValue = -80f;

    private void Start()
    {
        SetSliderValue(_sliders);

        _isMasterToogleOn = false;

        if (!_isMasterToogleOn)
        {
            _audioMixerGroup.audioMixer.SetFloat(MasterVolume, _minDCBValue);
        }
    }

    public void ToogleVolumeAll(bool isSoundOn)
    {
        _isMasterToogleOn = isSoundOn;

        if (isSoundOn)
        {
            _audioMixerGroup.audioMixer.SetFloat(MasterVolume, _maxDCBValue);
        }
        else
        {
            _audioMixerGroup.audioMixer.SetFloat(MasterVolume, _minDCBValue);
        }
    }

    public void ChangeVolumeAll(float volume)
    {
        if (_isMasterToogleOn)
            _audioMixerGroup.audioMixer.SetFloat(MasterVolume, Mathf.Log10(volume) * VolumeMultiple);
    }

    public void ChangeVolumeButtons(float volume)
    {
        if (_isMasterToogleOn)
        {
            _audioMixerGroup.audioMixer.SetFloat(ButtonVolume, Mathf.Log10(volume) * VolumeMultiple);
        }
    }

    public void ChangeVolumeFonMusic(float volume)
    {
        if (_isMasterToogleOn)
        {
            _audioMixerGroup.audioMixer.SetFloat(FonVolume, Mathf.Log10(volume) * VolumeMultiple);
        }
    }

    private void SetSliderValue(List<Slider> sliders)
    {
        float minValue = 0.0001f;
        float maxValue = 1f;

        foreach (var slider in sliders)
        {
            slider.minValue = minValue;
            slider.maxValue = maxValue;
        }
    }
}
