using UnityEngine;
using System;
using System.Collections;

public class OneBtnInput : MonoBehaviour
{
    [SerializeField] private PlayerController playerCtrlr;

    [Header("Button Settings")]
    public KeyCode actionKey = KeyCode.K;  // The single button used

    [Header("Timing Settings")]
    private float doubleTapTime = 0.2f;   // Max delay between taps for a double tap
    private float holdTime = 0.2f;        // Time needed to trigger a hold

    private float buttonDownTime = 0f;
    private bool isHolding = false;
    private bool buttonPressed = false;
    private int tapCount = 0;
    private Coroutine singleTapCoroutine;

    public event Action OnSingleClick;
    public event Action OnDoubleTap;
    public event Action OnHold;

    void Update()
    {
        // Button pressed
        if (Input.GetKeyDown(actionKey))
        {
            buttonPressed = true;
            buttonDownTime = Time.time;
            tapCount++;

            // Check for double tap
            if (tapCount == 2)
            {
                // Cancel waiting for single tap
                if (singleTapCoroutine != null)
                    StopCoroutine(singleTapCoroutine);

                OnDoubleTap?.Invoke();
                tapCount = 0;
            }
        }

        // Button released
        if (Input.GetKeyUp(actionKey))
        {
            buttonPressed = false;

            if (playerCtrlr.isGrounded)
            {
                OnSingleClick?.Invoke();
                tapCount = 0;
            }
            else if (!isHolding)
            {
                // Start waiting to see if this was a double tap or just a single tap
                if (tapCount == 1)
                {
                    if (singleTapCoroutine != null)
                        StopCoroutine(singleTapCoroutine);

                    singleTapCoroutine = StartCoroutine(SingleTapDelay());
                }
            }

            isHolding = false;
        }

        // Detect hold
        if (buttonPressed && !isHolding && Time.time - buttonDownTime >= holdTime)
        {
            tapCount = 0;
            isHolding = true;
            OnHold?.Invoke();
        }
    }

    IEnumerator SingleTapDelay()
    {
        // Wait to confirm no second tap happens
        yield return new WaitForSeconds(doubleTapTime);

        // If no second tap arrived in time, count as a single click
        if (tapCount == 1)
        {
            OnSingleClick?.Invoke();
        }

        tapCount = 0;
    }
}
