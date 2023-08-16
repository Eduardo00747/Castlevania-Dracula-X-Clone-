using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faca : MonoBehaviour
{
    private SpriteRenderer playerSpriteRenderer; // Refer�ncia ao SpriteRenderer do jogador

    private void Start()
    {
        // Obter a refer�ncia ao SpriteRenderer do jogador
        playerSpriteRenderer = FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>();
    }

    // Este m�todo � chamado quando um objeto com colisor de gatilho entra na �rea do colisor deste objeto
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar se a colis�o ocorreu com a tag "Player" ou "Borda"
        if (other.CompareTag("Borda") || other.CompareTag("Inimigo"))
        {
            // Destruir o objeto que possui este script (a Faca)
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Verificar o Flip X do jogador e aplicar ao objeto
        if (playerSpriteRenderer != null)
        {
            bool playerFlipX = playerSpriteRenderer.flipX;
            // Aplicar o mesmo estado de Flip X ao objeto
            GetComponent<SpriteRenderer>().flipX = playerFlipX;
        }
    }
}
