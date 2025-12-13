using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private LvlTimer lvlTimer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            lvlTimer.StopTimerAndFinish();
    }
}
