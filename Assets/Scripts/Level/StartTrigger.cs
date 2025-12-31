using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    [SerializeField] private GameObject InvisibleArea;
    [SerializeField] private Transform ModelParentTrans;

    //Audio
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            bc.isTrigger = false;

            InvisibleArea.SetActive(false);

            audioManager.PlaySFX(audioManager.bounce);

            foreach (Transform t in ModelParentTrans)
                t.gameObject.SetActive(true);
        }
    }
}
