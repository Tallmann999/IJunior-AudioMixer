using UnityEngine;
using UnityEngine.UI;

public class ButtonUi : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private AudioSource _audioSource;

    private void OnEnable()
    {
        _button.onClick.AddListener(HandleButtonClick);
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(HandleButtonClick);
    }

    private void HandleButtonClick()
    {
        _audioSource.Play();
    }
}
