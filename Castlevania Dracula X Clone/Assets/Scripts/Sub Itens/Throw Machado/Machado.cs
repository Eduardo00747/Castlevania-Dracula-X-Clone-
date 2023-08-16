using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machado : MonoBehaviour
{

    private SpriteRenderer playerSpriteRenderer; // Referência ao SpriteRenderer do jogador

    private void Start()
    {
        // Obter a referência ao SpriteRenderer do jogador
        playerSpriteRenderer = FindObjectOfType<PlayerController>().GetComponent<SpriteRenderer>();
    }
    // Este método é chamado quando um objeto com colisor de gatilho entra na área do colisor deste objeto
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar se a colisão ocorreu com a tag "Player" ou "Borda"
        if (other.CompareTag("Borda"))
        {
            // Destruir o objeto que possui este script (o Machado)
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
