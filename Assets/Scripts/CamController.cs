using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float transDuration = 0.3f; // seconds to complete transition
    private float camXOffset = 11f;
    private CinemachineVirtualCamera vCam;
    private CinemachineFramingTransposer camTransposer;
    private Coroutine flipCoroutine;

    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        camTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void FlipCameraOffset()
    {
        camXOffset *= -1;

        if (flipCoroutine != null)
            StopCoroutine(flipCoroutine);

        flipCoroutine = StartCoroutine(SmoothFlipOffset(camXOffset));
    }

    private IEnumerator SmoothFlipOffset(float targetX)
    {
        Vector3 startOffset = camTransposer.m_TrackedObjectOffset;
        float elapsed = 0f;

        while (elapsed < transDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transDuration);
            Vector3 newOffset = startOffset;
            newOffset.x = Mathf.Lerp(startOffset.x, targetX, t);
            camTransposer.m_TrackedObjectOffset = newOffset;
            yield return null;
        }

        Vector3 final = camTransposer.m_TrackedObjectOffset;
        final.x = targetX;
        camTransposer.m_TrackedObjectOffset = final;
    }
}

