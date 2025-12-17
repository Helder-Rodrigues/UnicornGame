using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField] private GameObject InvisibleArea;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            bc.isTrigger = false;

            InvisibleArea.SetActive(false);
        }
    }
}
