using UnityEngine;
using System;
using System.Collections;

public class OneBtnInput : MonoBehaviour
{
    [SerializeField] private PlayerController playerCtrlr;

    [Header("Button Settings")]
    public static KeyCode actionKey = KeyCode.Space;

    [Header("Timing Settings")]
    public float waitTime = 0.00f;
    private float lastTapTime = -1f;
    private float buttonDownStart = 0f;

    private Coroutine singleTapRoutine;
    private Coroutine holdRoutine;

    public event Action OnSingleClick;
    public event Action OnDoubleTap;
    public event Action OnHold;

    void Update()
    {
        if (PauseMenu.isPaused)
            return;

        if (Input.GetKeyDown(actionKey))
        {
            float time = Time.time;
            buttonDownStart = time;

            if (holdRoutine != null)
                StopCoroutine(holdRoutine);

            // If grounded - single click (no waiting)
            if (playerCtrlr.isGrounded)
            {
                // No tapCount, no double-tap window, no delay
                OnSingleClick?.Invoke();

                // Reset since we're not doing tap logic
                if (singleTapRoutine != null)
                    StopCoroutine(singleTapRoutine);
                lastTapTime = -1f;
                return;
            }

            // AIR INPUTS BELOW (DoubleTap / SingleTap / Hold)

            // DOUBLE TAP
            if (time - lastTapTime <= waitTime)
            {
                if (singleTapRoutine != null)
                    StopCoroutine(singleTapRoutine);
                if (holdRoutine != null)
                    StopCoroutine(holdRoutine);

                lastTapTime = -1f;

                OnDoubleTap?.Invoke();
                return;
            }

            lastTapTime = time;

            // HOLD CHECK - The Following ORDER Matters
            holdRoutine = StartCoroutine(HoldCheck(buttonDownStart));

            // SINGLE TAP - Start waiting for single tap
            singleTapRoutine = StartCoroutine(SingleTapCountdown());
        }

        if (holdRoutine != null && Input.GetKeyUp(actionKey))
            StopCoroutine(holdRoutine);
    }

    IEnumerator SingleTapCountdown()
    {
        yield return new WaitForSeconds(waitTime);

        OnSingleClick?.Invoke();
    }

    IEnumerator HoldCheck(float pressStartTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Make sure the same press is still active
        if (pressStartTime == buttonDownStart)
        {
            if (singleTapRoutine != null)
                StopCoroutine(singleTapRoutine);

            OnHold?.Invoke();
        }
    }
}
