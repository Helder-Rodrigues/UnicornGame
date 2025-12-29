using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public enum TutorialInputType
    {
        SingleClick,
        DoubleTap,
        Hold
    }

    [Header("References")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private OneBtnInput inputController;
    //[SerializeField] private LvlTimer lvlTimer;

    [Header("Tutorial Settings")]
    [SerializeField] private TutorialInputType inputType;
    [SerializeField] private GameObject tutBoxMessage;
    [TextArea][SerializeField] private string tutText;
    [SerializeField] private Vector3 tutBoxPos;
    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(TutorialRoutine());
        }
    }

    private IEnumerator TutorialRoutine()
    {
        bool continueTut = LockMostActions();
        if (!continueTut)
            yield break;

        playerController.FreezeMovement(true);

        GameObject boxMessage = null;
        if (tutBoxMessage != null)
        {
            boxMessage = Instantiate(tutBoxMessage, tutBoxPos + canvas.transform.position, Quaternion.identity, canvas.transform);

            TextMeshProUGUI message = boxMessage.GetComponentInChildren<TextMeshProUGUI>();
            if (message != null)
                message.text = tutText;
        }

        float tempMaxTitl = PlayerRotationController.maxTilt;
        PlayerRotationController.maxTilt = 0;
        
        bool inputReceived = false;
        void OnInput() => inputReceived = true;

        // Subscribe to the correct event
        Subscribe(OnInput);

        yield return new WaitUntil(() => inputReceived);

        Unsubscribe(OnInput);

        PlayerRotationController.maxTilt = tempMaxTitl;

        if (tutBoxMessage != null)
            Destroy(boxMessage);
    }

    private bool LockMostActions()
    {
        switch (inputType)
        {
            case TutorialInputType.SingleClick:
                if (playerController.doubleJumpDone)
                    return false;

                playerController.allowJump = true;
                playerController.allowDash = false;
                playerController.allowShield = false;
                break;

            case TutorialInputType.DoubleTap:
                if (playerController.isDashing)
                    return false;

                playerController.allowJump = false;
                playerController.allowDash = true;
                playerController.allowShield = false;
                break;

            case TutorialInputType.Hold:
                if (playerController.activeShield != null)
                    return false;

                playerController.allowJump = false;
                playerController.allowDash = false;
                playerController.allowShield = true;
                break;
        }

        return true;
    }

    private void Subscribe(System.Action handler)
    {
        switch (inputType)
        {
            case TutorialInputType.SingleClick:
                inputController.OnSingleClick += handler;
                break;

            case TutorialInputType.DoubleTap:
                inputController.OnDoubleTap += handler;
                break;

            case TutorialInputType.Hold:
                inputController.OnHold += handler;
                break;
        }
    }

    private void Unsubscribe(System.Action handler)
    {
        switch (inputType)
        {
            case TutorialInputType.SingleClick:
                inputController.OnSingleClick -= handler;
                break;

            case TutorialInputType.DoubleTap:
                inputController.OnDoubleTap -= handler;
                break;

            case TutorialInputType.Hold:
                inputController.OnHold -= handler;
                break;
        }
    }
}
