using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private GameObject deathFxPrefab;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private CamController camController;

    //Audio
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(DeathSequence(other));
    }

    private IEnumerator DeathSequence(Collider player)
    {
        audioManager.PlaySFX(audioManager.crowdBooing);

        camController.FollowTarget();

        GameObject fx = Instantiate(deathFxPrefab, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.05f);
        Destroy(player.gameObject);

        // Wait for the particle system to finish
        //ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        //while (ps.IsAlive(true))
        //    yield return null; // wait until the particle system is done

        yield return StartCoroutine(WaitForInputOrTime(OneBtnInput.actionKey, 3f));

        //Reload Level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator WaitForInputOrTime(KeyCode actionKey, float maxWaitTime)
    {
        bool inputReceived = false;
        float timer = 0f;

        while (!inputReceived && timer < maxWaitTime)
        {
            if (Input.GetKeyDown(actionKey))
                inputReceived = true;

            timer += Time.deltaTime;
            yield return null; // wait next frame
        }
    }
}
