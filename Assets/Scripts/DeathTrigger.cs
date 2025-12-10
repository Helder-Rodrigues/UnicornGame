using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private GameObject deathFxPrefab;
    [SerializeField] private PauseMenu pauseMenu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(DeathSequence(other));
    }

    private IEnumerator DeathSequence(Collider player)
    {
        GameObject fx = Instantiate(deathFxPrefab, player.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.05f);
        Destroy(player.gameObject);

        // Wait for the particle system to finish
        //ParticleSystem ps = fx.GetComponent<ParticleSystem>();
        //while (ps.IsAlive(true))
        //    yield return null; // wait until the particle system is done

        yield return new WaitForSeconds(3f);

        //Reload Level
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
