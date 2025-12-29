using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    public Transform model;          // drag your visual model here
    public Rigidbody rb;             // drag the player's Rigidbody
    public float rotateSpeed = 5f;   // smoothing speed
    public static float maxTilt = 90f;      // max rotation angle while moving in air
    public PlayerController playerCtrl; // script that contains isGrounded

    void Update()
    {
        RotateModel();
    }

    void RotateModel()
    {
        Vector3 vel = rb.velocity;

        if (playerCtrl.moveSpeed < 0)
            vel *= -1;

        float targetAngle = 0f;

        // If airborne, tilt based on horizontal velocity
        if (!playerCtrl.isGrounded)
        {
            // angle based on velocity direction
            targetAngle = Mathf.Clamp(Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg, -maxTilt, maxTilt);
        }

        // Smooth rotation on Z axis (like a 2D character in 3D)
        Quaternion desiredRot = Quaternion.Euler(0, 0, targetAngle);
        model.rotation = Quaternion.Lerp(model.rotation, desiredRot, Time.deltaTime * rotateSpeed);
    }
}

