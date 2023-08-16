using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowFaca : MonoBehaviour
{
    public GameObject facaPrefab; // Prefab da Faca
    public float facaSpeed = 15f; // Velocidade da Faca

    private SubWeapons subWeaponsScript; // Refer�ncia ao script SubWeapons
    private SpriteRenderer playerSpriteRenderer; // Refer�ncia ao SpriteRenderer do personagem "Ritcher"

    // Refer�ncia ao script PlayerController
    public PlayerController playerController;

    private void Start()
    {
        // Obter a refer�ncia ao script SubWeapons no objeto "Ritcher"
        subWeaponsScript = GetComponentInParent<SubWeapons>();

        // Obter a refer�ncia ao SpriteRenderer no objeto "Ritcher"
        playerSpriteRenderer = GetComponentInParent<SpriteRenderer>();
    }

    private void Update()
    {
        // Verificar se o jogador est� pressionando "Vertical Cima" (tecla W) e "K" para atirar a Faca
        if (playerController.coracoesColetados > 0 && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.K) && subWeaponsScript.equippedItem == ItemType.Faca)
        {
            // Instanciar o prefab da Faca no objeto "Throw Faca"
            if (facaPrefab != null)
            {
                GameObject facaInstance = Instantiate(facaPrefab, transform.position, Quaternion.identity);

                // Obter o Rigidbody2D da faca
                Rigidbody2D facaRigidbody = facaInstance.GetComponent<Rigidbody2D>();

                // Verificar se o Rigidbody2D foi encontrado e definir a velocidade da faca
                if (facaRigidbody != null)
                {
                    // Definir a velocidade da faca de acordo com a dire��o do sprite renderer
                    facaRigidbody.velocity = playerSpriteRenderer.flipX ? -transform.right * facaSpeed : transform.right * facaSpeed;

                    // Chamar a fun��o do PlayerController para subtrair 1 cora��o
                    playerController.SubtractHeart();
                }
            }
        }
    }
}
