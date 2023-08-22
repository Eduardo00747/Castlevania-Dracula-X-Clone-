using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AguaBenta : MonoBehaviour
{
    private bool collidedWithGround = false;

    public GameObject fogo1Prefab; // Prefab do objeto "Fogo 1"
    public GameObject fogo2Prefab; // Prefab do objeto "Fogo 1"
    public GameObject fogo3Prefab; // Prefab do objeto "Fogo 1"

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !collidedWithGround)
        {
            // Marcamos que colidimos com o chão para evitar múltiplas chamadas
            collidedWithGround = true;

            // Inicia a rotina para aguardar e destruir o objeto após um segundo
            StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        // Aguarda por um segundo
        yield return new WaitForSeconds(0.3f);

        // Instancia o objeto "Fogo 1"
        if (fogo1Prefab != null)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(fogo1Prefab, transform.position, Quaternion.identity);
        }

        // Instancia o objeto "Fogo 2"
        if (fogo2Prefab != null)
        {
            yield return new WaitForSeconds(0.11f);
            Instantiate(fogo2Prefab, transform.position, Quaternion.identity);
        }

        // Instancia o objeto "Fogo 3"
        if (fogo3Prefab != null)
        {
            yield return new WaitForSeconds(0.12f);
            Instantiate(fogo3Prefab, transform.position, Quaternion.identity);
        }

        // Destrói o objeto
        Destroy(gameObject);
    }
}