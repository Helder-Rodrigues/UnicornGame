using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private LvlTimer lvlTimer;

    public static bool isPaused = false;

    //Audio
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    
        TogglePauseMenu(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !isPaused)
            TogglePauseMenu();
    }

    private void TogglePauseMenu(bool? toPause = null)
    {
        if (toPause == null)
            toPause = !pauseMenu.activeSelf;
        isPaused = (bool)toPause;

        TogglePauseGame(isPaused);

        HidePauseMenu(!isPaused);
    }

    //Pauses GamePlay and Timer
    public void TogglePauseGame(bool? toPause = null)
    {
        if (toPause == null)
            toPause = !isPaused;
        isPaused = (bool)toPause;

        Time.timeScale = isPaused ? 0f : 1f;

        lvlTimer.ToggleTimer(!isPaused);
    }

    public void HidePauseMenu(bool hide) => pauseMenu.SetActive(!hide);

    public void OnContinueBtn()
    {
        audioManager.PlaySFX(audioManager.clickCardboard);
        TogglePauseMenu();
    }

    public void OnExitBtn()
    {
        audioManager.PlaySFX(audioManager.clickCardboard);
        TogglePauseMenu();
        SceneManager.LoadScene("LevelMenu");
    }
    
    public void OnOptionsBtn()
    {
        audioManager.PlaySFX(audioManager.clickCardboard);

        HidePauseMenu(true);

        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }
}
