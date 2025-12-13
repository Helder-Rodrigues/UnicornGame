using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recordLvl1Text;
    [SerializeField] private TextMeshProUGUI recordLvl2Text;
    [SerializeField] private TextMeshProUGUI recordLvl3Text;

    public static int recordLvl1Sec = -1;
    public static int recordLvl2Sec = -1;
    public static int recordLvl3Sec = -1;

    private void Start()
    {
        UpdateRecord(recordLvl1Sec, recordLvl1Text);
        UpdateRecord(recordLvl2Sec, recordLvl2Text);
        UpdateRecord(recordLvl3Sec, recordLvl3Text);
    }

    private void UpdateRecord(int seconds, TextMeshProUGUI targetText)
    {
        if (seconds <= 0) return;
        targetText.text = "Record:" + FormatTime(seconds);
    }

    private string FormatTime(int totalSeconds)
    {
        string result = "";

        // Minutes
        int min = totalSeconds / 60;
        if (min > 0)
            result += "\n" + min + " min";

        // Seconds (Roman style)
        int sec = totalSeconds % 60;
        if (sec > 0)
        {
            result += "\n";

            if (sec >= 50)
            {
                sec -= 50;
                result += "L";
            }
            else if (sec >= 40)
            {
                sec -= 40;
                result += "XL";
            }

            while (sec >= 10)
            {
                sec -= 10;
                result += "X";
            }

            if (sec > 0)
                result += sec.ToString();

            result += " sec";
        }

        return result;
    }

    public void OnclickLvl1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnclickLvl2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void OnclickLvl3()
    {
        SceneManager.LoadScene("Level3");
    }



}
