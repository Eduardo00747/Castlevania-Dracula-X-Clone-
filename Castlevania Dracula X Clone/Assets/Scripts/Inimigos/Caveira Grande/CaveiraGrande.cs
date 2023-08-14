using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveiraGrande : MonoBehaviour
{
    public float speed = 5f; // Velocidade do inimigo
    public float radius = 5f; // Raio de detec��o

    private Transform player; // Refer�ncia ao transform do jogador

    private SpriteRenderer spriteRenderer;


    void Start()
    {
        // Encontra o GameObject com a tag "Player" e pega seu transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        // Verifica se o jogador est� dentro do raio de detec��o
        if (Vector3.Distance(transform.position, player.position) <= radius)
        {
            // Calcula a dire��o para onde o inimigo deve se mover
            Vector3 direction = (player.position - transform.position).normalized;

            // Move o inimigo na dire��o do jogador com a velocidade definida
            transform.position += direction * speed * Time.deltaTime;
        }

        bool isPlayerOnRight = player.transform.position.x > transform.position.x;

        if (isPlayerOnRight)
        {
            spriteRenderer.flipX = true; // Inverte o sprite em X
        }
        else
        {
            spriteRenderer.flipX = false; // Restaura a orienta��o do sprite
        }
    }

}
