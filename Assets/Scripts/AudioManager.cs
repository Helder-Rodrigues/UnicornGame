using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("Audio Clips")]
    //Music
    public AudioClip levelMusic;
    
    //Actions
    public AudioClip bounce;
    public AudioClip dash;
    public AudioClip jump;

    //Other
    public AudioClip applause;
    public AudioClip crowdBooing;
    public AudioClip clickCardboard;

    //Not used for now
    public AudioClip snortingHorse;
    public AudioClip snortingHorseDying;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Instance.PlayMusic(musicSource.clip);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
