using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip clickAudioClip;
    [SerializeField] private AudioClip matchAudioClip;
    [SerializeField] private AudioClip wrongAudioClip;
    [SerializeField] private AudioClip victoryAudioClip;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(clickAudioClip);
    }

    public void PlayMatchSound()
    {
        _audioSource.Stop();
        _audioSource.clip = matchAudioClip;
        _audioSource.Play();
    }

    public void PlayWrongSound()
    {
        _audioSource.Stop();
        _audioSource.clip = wrongAudioClip;
        _audioSource.Play();
    }

    public void PlayVictorySound()
    {
        _audioSource.Stop();
        _audioSource.clip = victoryAudioClip;
        _audioSource.Play();
    }
}