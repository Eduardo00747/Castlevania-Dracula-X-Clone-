using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogoAgua : MonoBehaviour
{
    public float moveSpeed = 1.0f; // Velocidade de movimento
    private Vector3 movementDirection; // Direção de movimento

    private PlayerController playerController; // Referência ao script PlayerController
    private SpriteRenderer playerSpriteRenderer; // Referência ao SpriteRenderer do jogador
    private bool startedMoving = false; // Indica se o objeto começou a se mover

    // Start is called before the first frame update
    void Start()
    {
        // Destruir o objeto após 3 segundos
        Destroy(gameObject, 1.5f);

        // Obter a referência ao PlayerController no objeto do jogador
        playerController = FindObjectOfType<PlayerController>();

        // Obter a referência ao SpriteRenderer no objeto do jogador
        playerSpriteRenderer = playerController.GetComponent<SpriteRenderer>();

        // Inicialmente, a direção de movimento é para a direita
        movementDirection = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        // Se o objeto ainda não começou a se mover
        if (!startedMoving)
        {
            // Verificar se o jogador está virado para a esquerda (flipX true)
            if (playerSpriteRenderer.flipX)
            {
                // Inicializa a direção de movimento para a esquerda
                movementDirection = Vector3.left;
            }

            // Indicar que o objeto começou a se mover
            startedMoving = true;
        }

        // Calcula a quantidade de movimento
        float movement = moveSpeed * Time.deltaTime;

        // Move o objeto na direção definida
        transform.Translate(movementDirection * movement);
    }
}