using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    [SerializeField] private Renderer objectRenderer;
    [SerializeField] private float blinkDuration = 1.5f;
    [SerializeField] private float blinkInterval = 0.2f;
    [SerializeField] private float destroyDelay = 0.3f;

    private void Start()
    {
        StartCoroutine(BlinkAndDestroy());
    }

    private IEnumerator BlinkAndDestroy()
    {
        yield return new WaitForSeconds(blinkDuration);

        // Ativar e desativar o Renderer para fazer o objeto piscar
        for (int i = 0; i < 5; i++)
        {
            objectRenderer.enabled = !objectRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Aguardar mais um tempo antes de destruir o objeto
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}