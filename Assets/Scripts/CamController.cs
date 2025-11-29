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

        currentZoom = normalFOV;
        vCam.m_Lens.FieldOfView = currentZoom;
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

    // Camera Air Zoom
    [Header("References")]
    [SerializeField] private Rigidbody playerRb;

    [Header("Zoom Settings")]
    [SerializeField] private float normalFOV = 40f;
    [SerializeField] private float maxZoomOutFOV = 60f;
    [SerializeField] private float zoomOutSpeed = 1f;  // how fast zooms out
    [SerializeField] private float zoomInSpeed = 3f;   // how fast zoom goes back to normal

    [Header("Air Settings")]
    [SerializeField] private float airTimeToMaxZoom = 1.0f; // seconds airborne to reach max zoom

    private float airTime = 0f;
    private float currentZoom;

    private bool isGrounded = true;

    private void Update()
    {
        UpdateAirTime();
        UpdateZoom();
    }

    private void UpdateAirTime()
    {
        // Aqui você coloca sua lógica real de grounded.
        // Exemplo rápido:
        isGrounded = Physics.Raycast(playerRb.transform.position, Vector3.down, 1.1f);

        if (!isGrounded)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            airTime -= Time.deltaTime * 3f; // volta mais rápido
        }

        airTime = Mathf.Clamp(airTime, 0f, airTimeToMaxZoom);
    }

    private void UpdateZoom()
    {
        float t = airTime / airTimeToMaxZoom;  // 0 → no chão, 1 → máximo no ar
        float targetZoom = Mathf.Lerp(normalFOV, maxZoomOutFOV, t);

        if (isGrounded)
        {
            // zoom-in rápido
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomInSpeed);
        }
        else
        {
            // zoom-out lento
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomOutSpeed);
        }

        vCam.m_Lens.FieldOfView = currentZoom;
    }
}

