using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HistoryManager : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private float fadeOutMusicDuration;
    private AudioManager audioManager;
    private AudioSource musicSource;

    private float startVolume;

    [Header("RawImage & Textures")]
    [SerializeField] private RawImage rawImageSlides;
    [SerializeField] private Texture[] introTexs;
    [SerializeField] private Texture[] endingTexs;

    private static bool introDone = false;
    private static bool endingDone = false;
    private Texture[] texsToUse;
    private int currTex = 0;

    [Header("Other References")]
    [SerializeField] private TextMeshProUGUI helperPress;

    private void Start()
    {
        if (endingDone)
        {
            SceneManager.LoadScene("LevelMenu");
            SceneManager.UnloadSceneAsync("History");
            return;
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        musicSource = audioManager.gameObject.GetComponentInChildren<AudioSource>();

        startVolume = musicSource.volume;

        if (!introDone)
        {
            audioManager.PlayMusic(audioManager.audienceBeforeShow);
            StartCoroutine(FadeOutMusic());
        }
        else
            helperPress.gameObject.SetActive(false);

        texsToUse = introDone ? endingTexs : introTexs;
        rawImageSlides.gameObject.SetActive(true);
        rawImageSlides.texture = texsToUse[currTex];
    }

    private IEnumerator FadeOutMusic()
    {
        float elapsed = 0f;

        while (elapsed < fadeOutMusicDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeOutMusicDuration);
            float newVolume = Mathf.Lerp(startVolume, 0, t);
            musicSource.volume = newVolume;
            yield return null;
        }

        musicSource.volume = 0f;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (currTex == 0)
            {
                if (introDone)
                    endingDone = true;
                else
                {
                    introDone = true;
                    Destroy(helperPress.gameObject);
                }
            }

            currTex++;

            if (currTex < texsToUse.Length)
                rawImageSlides.texture = texsToUse[currTex];
            else
            {
                musicSource.volume = startVolume;
                SceneManager.LoadScene("LevelMenu");
                SceneManager.UnloadSceneAsync("History");
            }
        }
    }
}
