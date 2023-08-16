using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowAguaBenta : MonoBehaviour
{
    public GameObject aguaBentaPrefab; // Prefab da �gua Benta
    public float aguaBentaSpeed = 10f; // Velocidade da �gua Benta

    private SubWeapons subWeaponsScript; // Refer�ncia ao script SubWeapons
    public PlayerController playerController; // Refer�ncia ao script PlayerController
    private SpriteRenderer playerSpriteRenderer; // Refer�ncia ao SpriteRenderer do jogador

    private void Start()
    {
        // Obter a refer�ncia ao script SubWeapons no objeto "Ritcher"
        subWeaponsScript = GetComponentInParent<SubWeapons>();

        // Obter a refer�ncia ao script PlayerController no objeto "Ritcher"
        playerController = GetComponentInParent<PlayerController>();

        // Obter a refer�ncia ao SpriteRenderer no objeto do jogador
        playerSpriteRenderer = playerController.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Verificar se o jogador est� pressionando "Vertical Cima" (tecla W) e "K" para atirar a �gua Benta
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.K) && subWeaponsScript.equippedItem == ItemType.AguaBenta)
        {
            // Instanciar o prefab da �gua Benta no objeto atual
            if (aguaBentaPrefab != null)
            {
                GameObject aguaBentaInstance = Instantiate(aguaBentaPrefab, transform.position, Quaternion.identity);

                // Obter o Rigidbody2D do prefab da �gua Benta
                Rigidbody2D aguaBentaRigidbody = aguaBentaInstance.GetComponent<Rigidbody2D>();

                // Verificar se o Rigidbody2D foi encontrado e definir a velocidade da �gua Benta
                if (aguaBentaRigidbody != null)
                {
                    // Ajustar a velocidade horizontal para inclinar para a direita se o Flip X do SpriteRenderer do jogador for verdadeiro
                    float horizontalSpeed = playerSpriteRenderer.flipX ? -1.6f : 1.6f;
                    Vector2 adjustedVelocity = new Vector2(horizontalSpeed, -1f).normalized * aguaBentaSpeed;
                    aguaBentaRigidbody.velocity = adjustedVelocity;

                    // Chamar a fun��o do PlayerController para subtrair recursos (por exemplo, cora��es)
                    playerController.SubtractHeart();
                }
            }
        }
    }
}