using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAguaBenta : MonoBehaviour
{
    public GameObject aguaBentaPrefab; // Prefab da Água Benta
    public float aguaBentaSpeed = 10f; // Velocidade da Água Benta

    private SubWeapons subWeaponsScript; // Referência ao script SubWeapons
    public PlayerController playerController; // Referência ao script PlayerController
    private SpriteRenderer playerSpriteRenderer; // Referência ao SpriteRenderer do jogador

    private void Start()
    {
        // Obter a referência ao script SubWeapons no objeto "Ritcher"
        subWeaponsScript = GetComponentInParent<SubWeapons>();

        // Obter a referência ao script PlayerController no objeto "Ritcher"
        playerController = GetComponentInParent<PlayerController>();

        // Obter a referência ao SpriteRenderer no objeto do jogador
        playerSpriteRenderer = playerController.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Verificar se o jogador está pressionando "Vertical Cima" (tecla W) e "K" para atirar a Água Benta
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.K) && subWeaponsScript.equippedItem == ItemType.AguaBenta)
        {
            // Instanciar o prefab da Água Benta no objeto atual
            if (aguaBentaPrefab != null)
            {
                GameObject aguaBentaInstance = Instantiate(aguaBentaPrefab, transform.position, Quaternion.identity);

                // Obter o Rigidbody2D do prefab da Água Benta
                Rigidbody2D aguaBentaRigidbody = aguaBentaInstance.GetComponent<Rigidbody2D>();

                // Verificar se o Rigidbody2D foi encontrado e definir a velocidade da Água Benta
                if (aguaBentaRigidbody != null)
                {
                    // Ajustar a velocidade horizontal para inclinar para a direita se o Flip X do SpriteRenderer do jogador for verdadeiro
                    float horizontalSpeed = playerSpriteRenderer.flipX ? -1.6f : 1.6f;
                    Vector2 adjustedVelocity = new Vector2(horizontalSpeed, -1f).normalized * aguaBentaSpeed;
                    aguaBentaRigidbody.velocity = adjustedVelocity;

                    // Chamar a função do PlayerController para subtrair recursos (por exemplo, corações)
                    playerController.SubtractHeart();
                }
            }
        }
    }
}