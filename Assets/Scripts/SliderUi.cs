using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderUi : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private VolumeType _type;

    public event Action<VolumeType, float> VolumeChanged;

    public float Value => _slider.value;
    public VolumeType Type => _type;
    public string Key => Type.ToString();

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }

    private void OnSliderValueChanged(float value)
    {
        VolumeChanged?.Invoke(_type, value);
    }
}
