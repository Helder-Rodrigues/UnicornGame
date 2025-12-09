using UnityEngine;
using Cinemachine;

public class CameraJumpZoom : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerController playerCtrlr;

    [Header("Zoom Settings")]
    [SerializeField] private float normalFOV = 80f;
    [SerializeField] private float maxZoomOutFOV = 100f;
    [SerializeField] private float zoomOutSpeed = 1f;
    [SerializeField] private float zoomInSpeed = 3f;
    [SerializeField] private float groundedZoomInSpeed = 6f;

    [Header("Jump Timing")]
    [SerializeField] private float requiredAirUpTime = 2f; // Tempo subindo em Y até começar o zoom

    private float currentZoom;
    private float airUpTime = 0f;
    private bool zoomingOut = false;
    private bool zoomingIn = false;

    private void Start()
    {
        currentZoom = normalFOV;
        vcam.m_Lens.FieldOfView = currentZoom;
    }

    private void Update()
    {
        HandleVerticalMovementState();
        UpdateCameraZoom();
    }

    private void HandleVerticalMovementState()
    {
        float yVel = rb.velocity.y;

        // Se está a subir (yVel > 0)
        if (!playerCtrlr.isGrounded && yVel > 0f)
        {
            airUpTime += Time.deltaTime;

            // Começa o zoom OUT só depois de subir X segundos
            if (airUpTime >= requiredAirUpTime)
            {
                zoomingOut = true;
                zoomingIn = false;
            }
        }
        else
        {
            // Parou de subir ou está no chão → reset de contador
            airUpTime = 0f;

            // Se está a descer ou grounded → inicia zoom IN
            if (zoomingOut)
            {
                zoomingIn = true;
                zoomingOut = false;
            }
        }

        // Se encostou no chão → zoom in mais rápido
        if (playerCtrlr.isGrounded)
        {
            zoomingIn = true;
            zoomingOut = false;
        }
    }

    private void UpdateCameraZoom()
    {
        float targetZoom = normalFOV;

        if (zoomingOut)
            targetZoom = maxZoomOutFOV;

        if (zoomingIn)
            targetZoom = normalFOV;

        // Velocidades diferentes dependendo do estado
        float speed =
            (zoomingOut ? zoomOutSpeed :
            playerCtrlr.isGrounded ? groundedZoomInSpeed :
            zoomInSpeed);

        // Smooth interpolate
        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * speed);

        vcam.m_Lens.FieldOfView = currentZoom;
    }
}
