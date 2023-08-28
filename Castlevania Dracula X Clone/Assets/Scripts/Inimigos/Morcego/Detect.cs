using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect : MonoBehaviour
{
    private Rigidbody2D morcegoRigidbody; // Referência ao Rigidbody2D do objeto "Morcego"

    private void Start()
    {
        // Obtém a referência ao Rigidbody2D no objeto pai ("Morcego")
        morcegoRigidbody = GetComponentInParent<Rigidbody2D>();
    }

    // Chamado quando um Collider entra no Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o Collider que entrou é o "Player"
        if (other.CompareTag("Player"))
        {
            // Altera a gravidade do objeto "Morcego" para 1 (habilitada)
            morcegoRigidbody.gravityScale = 1f;

            // Destruir o objeto que possui este script
            Destroy(gameObject);
        }
    }

}
