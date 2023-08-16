using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cruz : MonoBehaviour
{
    // Este m�todo � chamado quando um objeto com colisor de gatilho entra na �rea do colisor deste objeto
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar se a colis�o ocorreu com a tag "Player" ou "Borda"
        if (other.CompareTag("Player") || other.CompareTag("Borda"))
        {
            // Destruir o objeto que possui este script (o Machado)
            Destroy(gameObject);
        }
    }
}
