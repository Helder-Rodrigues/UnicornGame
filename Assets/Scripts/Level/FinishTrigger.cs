using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private LvlTimer lvlTimer;
    
    //Audio
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            audioManager.PlaySFX(audioManager.applause);
            lvlTimer.StopTimerAndFinish();
        }
    }
}
