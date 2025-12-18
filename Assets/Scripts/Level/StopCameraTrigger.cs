using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCameraTrigger : MonoBehaviour
{
    [SerializeField] private CamController camController;
    [SerializeField] private Transform unicornTrans;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            camController.FollowTarget();
    }

    private void OnTriggerStay(Collider other)
    {
        if (camController.GetCamFollowTarget() != null && other.CompareTag("Player"))
            camController.FollowTarget();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject != null && unicornTrans != null)
                camController.FollowTarget(unicornTrans);
        }
    }
}
