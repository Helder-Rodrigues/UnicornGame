using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class CamController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float flipTransDuration = 0.3f; // seconds to complete transition
    private float camXOffset;
    private CinemachineVirtualCamera vCam;
    private CinemachineFramingTransposer camTransposer;
    private Coroutine flipCoroutine;
    
    private void Start()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        camTransposer = vCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        camXOffset = camTransposer.m_TrackedObjectOffset.x;
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

        while (elapsed < flipTransDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / flipTransDuration);
            Vector3 newOffset = startOffset;
            newOffset.x = Mathf.Lerp(startOffset.x, targetX, t);
            camTransposer.m_TrackedObjectOffset = newOffset;
            yield return null;
        }

        Vector3 final = camTransposer.m_TrackedObjectOffset;
        final.x = targetX;
        camTransposer.m_TrackedObjectOffset = final;
    }

    public void FollowTarget(Transform target = null)
    {
        vCam.Follow = target;
    }
}