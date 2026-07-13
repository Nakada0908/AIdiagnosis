using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource seAudioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmAudioSource.clip == clip)
        {
            return;
        }

        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
        bgmAudioSource.loop = true;
    }

    public void PlaySE(AudioClip clip)
    {
        seAudioSource.PlayOneShot(clip);
    }

    public void PlayVoice(AudioClip clip)
    {
        seAudioSource.PlayOneShot(clip);
    }
}