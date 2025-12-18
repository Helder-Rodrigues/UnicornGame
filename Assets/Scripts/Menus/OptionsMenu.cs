using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private PauseMenu pauseMenu;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    //Audio
    private AudioManager audioManager;
    List<AudioSource> filteredSources = new();
    private bool changingValue = false;
    private bool mouseClicked = false;

    private void Start()
    {
        pauseMenu = FindFirstObjectByType<PauseMenu>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        // Carregar valores salvos
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        changingValue = false;

        AudioSource[] allSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allSources)
        {
            if (source.gameObject.transform.parent.name != "Audio Manager")
            {
                source.enabled = false;
                filteredSources.Add(source);
            }
        }
    }

    public void SetMusicVolume(float value)
    {
        changingValue = true;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        changingValue = true;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClicked = true;
            changingValue = false;
        }

        if (changingValue && mouseClicked)
        {
            audioManager.PlaySFX(audioManager.clickCardboard);
            mouseClicked = false;
        }

        if (Input.GetMouseButtonUp(0))
            changingValue = false;
    }

    public void OnExitBtn()
    {
        audioManager.PlaySFX(audioManager.clickCardboard);

        audioManager.PlayMusic(audioManager.levelMusic);
        foreach (AudioSource source in filteredSources)
            source.enabled = true;

        Time.timeScale = 0f;
        pauseMenu.HidePauseMenu(false);
        pauseMenu.LockAndFreezePlayer(false);

        SceneManager.UnloadSceneAsync("OptionsMenu");
    }
}