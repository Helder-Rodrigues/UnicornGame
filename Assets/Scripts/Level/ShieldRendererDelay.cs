using System.Collections;
using UnityEngine;

public class ShieldRendererDelay : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine(EnableRendererNextFrame());
    }

    IEnumerator EnableRendererNextFrame()
    {
        var r = GetComponent<Renderer>();
        if (r != null)
        {
            r.enabled = false;
            yield return null; // espera 1 frame
            r.enabled = true;
        }
    }
}
