using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera introCam;
    [SerializeField] private CinemachineVirtualCamera gameplayCam;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private LvlTimer lvlTimer;
    [SerializeField] private float blendTime;
    private bool switched = false;

    private void Start()
    {
        introCam.Priority = 20;
        gameplayCam.Priority = 10;

        playerController.FreezeMovement(true);
        PauseMenu.isPaused = true;
    }

    private void Update()
    {
        if (!switched && Input.GetKeyDown(OneBtnInput.actionKey))
        {
            StartCoroutine(SwitchAndUnpause());
        }
    }

    private IEnumerator SwitchAndUnpause()
    {
        SwitchToGameplay();
        yield return new WaitForSeconds(blendTime);

        PauseMenu.isPaused = false;
        lvlTimer.ResetTimer();
        playerController.FreezeMovement(false);
    }

    private void SwitchToGameplay()
    {
        gameplayCam.Priority = 30;
        introCam.Priority = 0;
        switched = true;
    }
}
