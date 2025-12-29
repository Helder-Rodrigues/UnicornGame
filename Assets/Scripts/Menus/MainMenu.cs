using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private bool keyChosen = false;

    private void Start()
    {
        // Carregar valores salvos
        float musicVolValue = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        float sfxVolValue = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolValue) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolValue) * 20);
    }

    private void Update()
    {
        if (keyChosen)
            return;

        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKeyDown(key))
            {
                keyChosen = true;
                OneBtnInput.actionKey = key;
                SceneManager.LoadScene("History", LoadSceneMode.Additive);
            }
    }
}
