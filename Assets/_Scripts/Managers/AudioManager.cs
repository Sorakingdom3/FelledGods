using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetVolume(float value)
    {
        _audioSource.volume = value;
    }

    public float GetVolume()
    {
        return _audioSource.volume;
    }
}
