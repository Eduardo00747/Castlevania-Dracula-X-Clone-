using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogoAgua : MonoBehaviour
{
    public float moveSpeed = 1.0f; // Velocidade de movimento
    private Vector3 movementDirection; // Dire��o de movimento

    private PlayerController playerController; // Refer�ncia ao script PlayerController
    private SpriteRenderer playerSpriteRenderer; // Refer�ncia ao SpriteRenderer do jogador
    private bool startedMoving = false; // Indica se o objeto come�ou a se mover

    // Start is called before the first frame update
    void Start()
    {
        // Destruir o objeto ap�s 3 segundos
        Destroy(gameObject, 1.5f);

        // Obter a refer�ncia ao PlayerController no objeto do jogador
        playerController = FindObjectOfType<PlayerController>();

        // Obter a refer�ncia ao SpriteRenderer no objeto do jogador
        playerSpriteRenderer = playerController.GetComponent<SpriteRenderer>();

        // Inicialmente, a dire��o de movimento � para a direita
        movementDirection = Vector3.right;
    }

    // Update is called once per frame
    void Update()
    {
        // Se o objeto ainda n�o come�ou a se mover
        if (!startedMoving)
        {
            // Verificar se o jogador est� virado para a esquerda (flipX true)
            if (playerSpriteRenderer.flipX)
            {
                // Inicializa a dire��o de movimento para a esquerda
                movementDirection = Vector3.left;
            }

            // Indicar que o objeto come�ou a se mover
            startedMoving = true;
        }

        // Calcula a quantidade de movimento
        float movement = moveSpeed * Time.deltaTime;

        // Move o objeto na dire��o definida
        transform.Translate(movementDirection * movement);
    }
}