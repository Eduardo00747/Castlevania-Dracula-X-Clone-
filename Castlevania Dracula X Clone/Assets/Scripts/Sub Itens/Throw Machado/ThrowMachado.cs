using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowMachado : MonoBehaviour
{
    public GameObject machadoPrefab; // Prefab do Machado
    public float machadoSpeed = 10f; // Velocidade do Machado

    private SubWeapons subWeaponsScript; // Referência ao script SubWeapons
    private SpriteRenderer playerSpriteRenderer; // Referência ao SpriteRenderer do personagem "Ritcher"

    // Referência ao script PlayerController
    public PlayerController playerController;

    private void Start()
    {
        // Obter a referência ao script SubWeapons no objeto "Ritcher"
        subWeaponsScript = GetComponentInParent<SubWeapons>();

        // Obter a referência ao SpriteRenderer no objeto "Ritcher"
        playerSpriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Verificar se o jogador está pressionando "Vertical Cima" (tecla W) e "K" para atirar o Machado
        if (playerController.coracoesColetados > 0 && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.K) && subWeaponsScript.equippedItem == ItemType.Machado)
        {
            // Instanciar o prefab "Machado" no objeto "Throw Machado"
            if (machadoPrefab != null)
            {
                GameObject machadoInstance = Instantiate(machadoPrefab, transform.position, Quaternion.identity);

                // Obter o Rigidbody2D do prefab
                Rigidbody2D machadoRigidbody = machadoInstance.GetComponent<Rigidbody2D>();

                // Verificar se o Rigidbody2D foi encontrado e definir a velocidade do Machado
                if (machadoRigidbody != null)
                {
                    // Calcular a direção do movimento oblíquo
                    Vector2 launchDirection = new Vector2(1f, 1f);

                    // Verificar a direção do flip do sprite renderer e ajustar a velocidade do machado
                    if (playerSpriteRenderer.flipX)
                    {
                        // Sprite está em flip (olhando para a esquerda)
                        launchDirection.x = -1f; // Inverter a direção em X
                    }

                    // Definir a velocidade do machado com a direção do movimento oblíquo
                    machadoRigidbody.velocity = new Vector2(launchDirection.x * machadoSpeed, launchDirection.y * machadoSpeed);

                    // Chamar a função do PlayerController para subtrair 1 coração
                    playerController.SubtractHeart();
                }
            }
        }
    }
}
