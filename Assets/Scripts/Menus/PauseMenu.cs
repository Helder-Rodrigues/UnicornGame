using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private LvlTimer lvlTimer;
    [SerializeField] private PlayerController playerController;

    public static bool isPaused = false;
    private float defaultMaxTitl;

    //Audio
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        defaultMaxTitl = PlayerRotationController.maxTilt;

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

        LockAndFreezePlayer(isPaused);

        PlayerRotationController.maxTilt = isPaused ? 0 : defaultMaxTitl;

        lvlTimer.ToggleTimer(!isPaused);
    }

    public void HidePauseMenu(bool hide) => pauseMenu.SetActive(!hide);

    public void LockAndFreezePlayer(bool lockAndFreeze)
    {
        playerController.allowJump = !lockAndFreeze;
        playerController.allowDash = !lockAndFreeze;
        playerController.allowShield = !lockAndFreeze;

        playerController.FreezeMovement(lockAndFreeze);
    }

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

        LockAndFreezePlayer(true);
        HidePauseMenu(true);
        Time.timeScale = 1f;

        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }
}
