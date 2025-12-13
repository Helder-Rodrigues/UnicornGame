using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlTimer : MonoBehaviour
{
    public static float levelTime = 0f;   // time result stored here
    private bool timerRunning = false;

    private void Start()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        ToggleTimer(false);
        levelTime = 0;
        ToggleTimer(true);
    }

    void Update()
    {
        // COUNTING time
        if (timerRunning)
            levelTime += Time.deltaTime;
    }

    public void ToggleTimer(bool? value = null)
    {
        if (value != null)
        {
            timerRunning = (bool)value;
            return;
        }

        timerRunning = !timerRunning;
    }

    // Called by the FinishTrigger
    public void StopTimerAndFinish()
    {
        ToggleTimer(false);

        if (SceneManager.GetActiveScene().name.Contains("1"))
        {
            if (LevelMenu.recordLvl1Sec > (int)levelTime || LevelMenu.recordLvl1Sec <= 0)
                LevelMenu.recordLvl1Sec = (int)levelTime;
        }
        else if (SceneManager.GetActiveScene().name.Contains("2"))
        {
            if (LevelMenu.recordLvl2Sec > (int)levelTime || LevelMenu.recordLvl2Sec <= 0)
                LevelMenu.recordLvl2Sec = (int)levelTime;
        }
        else
        {
            if (LevelMenu.recordLvl3Sec > (int)levelTime || LevelMenu.recordLvl3Sec <= 0)
                LevelMenu.recordLvl3Sec = (int)levelTime;
        }

        SceneManager.LoadScene("LevelMenu");
    }
}
