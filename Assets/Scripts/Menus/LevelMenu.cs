using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    private Vignette vignette;

    [SerializeField] private TextMeshProUGUI recordLvl1Text;
    [SerializeField] private TextMeshProUGUI recordLvl2Text;
    [SerializeField] private TextMeshProUGUI recordLvl3Text;

    public static int recordLvl1Sec = -1;
    public static int recordLvl2Sec = -1;
    public static int recordLvl3Sec = -1;

    //Audio
    private AudioManager audioManager;

    private void Awake()
    {
        if (globalVolume.profile.TryGet(out vignette) == false)
            Debug.LogError("Vignette not found in Volume Profile!");
    }

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
     
        UpdateRecord(recordLvl1Sec, recordLvl1Text);
        UpdateRecord(recordLvl2Sec, recordLvl2Text);
        UpdateRecord(recordLvl3Sec, recordLvl3Text);
    }

    void Update()
    {
        if (vignette == null)
            return;

        // Mouse position in pixels
        Vector2 mousePos = Input.mousePosition;

        // Normalize
        float x = Mathf.Clamp01(mousePos.x / Screen.width);
        float y = Mathf.Clamp01(mousePos.y / Screen.height);

        // Set vignette center
        vignette.center.value = new Vector2(x, y);
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
        audioManager.PlaySFX(audioManager.clickCardboard);
        SceneManager.LoadScene("Level1");
    }

    public void OnclickLvl2()
    {
        audioManager.PlaySFX(audioManager.clickCardboard);
        SceneManager.LoadScene("Level2");
    }

    public void OnclickLvl3()
    {
        audioManager.PlaySFX(audioManager.clickCardboard);
        SceneManager.LoadScene("Level3");
    }
}
