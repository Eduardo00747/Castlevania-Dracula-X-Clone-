using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItem : MonoBehaviour
{
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        StartCoroutine(BlinkAndDestroy());
    }

    private IEnumerator BlinkAndDestroy()
    {
        yield return new WaitForSeconds(1.5f);

        // Ativar e desativar o Renderer para fazer o objeto piscar
        for (int i = 0; i < 5; i++)
        {
            objectRenderer.enabled = !objectRenderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }

        // Aguardar mais 0,3 segundos antes de destruir o objeto
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }
}
