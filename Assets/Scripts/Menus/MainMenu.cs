using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Update()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKeyDown(key))
            {
                OneBtnInput.actionKey = key;
                SceneManager.LoadScene("LevelMenu");
            }
    }
}
