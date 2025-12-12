using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (Input.GetKeyUp(KeyCode.Escape))
            TogglePauseMenu();
    }

    private void TogglePauseMenu(bool? value = null)
    {
        if (value == null)
            value = !pauseMenu.activeSelf;
        isPaused = (bool)value;

        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        lvlTimer.ToggleTimer(!isPaused);

        pauseMenu.SetActive(isPaused);
    }

    public void OnContinueBtn()
    {
        TogglePauseMenu();
    }

    public void OnExitBtn()
    {
        SceneManager.LoadScene("LevelMenu");
    }
}
