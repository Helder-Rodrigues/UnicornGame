using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private LvlTimer lvlTimer;

    public static bool isPaused = false;

    private void Start()
    {
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

        pauseMenu.SetActive(isPaused);
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

    public void OnContinueBtn()
    {
        TogglePauseMenu();
    }

    public void OnExitBtn()
    {
        isPaused = false;
        SceneManager.LoadScene("LevelMenu");
    }
}
