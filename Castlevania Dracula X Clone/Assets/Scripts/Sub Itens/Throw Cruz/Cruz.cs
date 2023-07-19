using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruz : MonoBehaviour
{
    // Este m�todo � chamado quando o objeto colide com outro collider 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se a colis�o ocorreu com a tag "Player" ou "Borda"
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Borda"))
        {
            // Destruir o objeto que possui este script (a Cruz)
            Destroy(gameObject);
        }
    }
}
