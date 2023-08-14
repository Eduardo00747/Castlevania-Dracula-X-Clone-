using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveiraGrande : MonoBehaviour
{
    public float speed = 5f; // Velocidade do inimigo
    public float radius = 5f; // Raio de detecção

    private Transform player; // Referência ao transform do jogador

    private SpriteRenderer spriteRenderer;


    void Start()
    {
        // Encontra o GameObject com a tag "Player" e pega seu transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        // Verifica se o jogador está dentro do raio de detecção
        if (Vector3.Distance(transform.position, player.position) <= radius)
        {
            // Calcula a direção para onde o inimigo deve se mover
            Vector3 direction = (player.position - transform.position).normalized;

            // Move o inimigo na direção do jogador com a velocidade definida
            transform.position += direction * speed * Time.deltaTime;
        }

        bool isPlayerOnRight = player.transform.position.x > transform.position.x;

        if (isPlayerOnRight)
        {
            spriteRenderer.flipX = true; // Inverte o sprite em X
        }
        else
        {
            spriteRenderer.flipX = false; // Restaura a orientação do sprite
        }
    }

}
