using UnityEngine;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPlay()
    {
        _audioSource.Play();
    }
}
