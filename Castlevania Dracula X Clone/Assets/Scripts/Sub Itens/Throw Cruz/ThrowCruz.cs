using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCruz : MonoBehaviour
{
    public GameObject cruzAPrefab; // Prefab da Cruz A
    public float cruzASpeed = 10f; // Velocidade da Cruz A

    private SubWeapons subWeaponsScript; // Referência ao script SubWeapons

    // Referência à contagem de corações do script PlayerController
    public PlayerController playerController;

    private void Start()
    {
        // Obter a referência ao script SubWeapons no objeto "Ritcher"
        subWeaponsScript = GetComponentInParent<SubWeapons>();
    }

    // Update is called once per frame
    void Update()
    {
        // Verificar se o jogador está pressionando "Vertical Cima" (tecla W) e "K" para atirar a Cruz A
        if (playerController.coracoesColetados > 0 && Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.K) && subWeaponsScript.equippedItem == ItemType.Cruz)
        {
            // Instanciar o prefab "Cruz A" no objeto "Throw Cruz"
            if (cruzAPrefab != null)
            {
                GameObject cruzAInstance = Instantiate(cruzAPrefab, transform.position, Quaternion.identity);

                // Obter o Rigidbody2D do prefab
                Rigidbody2D cruzARigidbody = cruzAInstance.GetComponent<Rigidbody2D>();

                // Verificar se o Rigidbody2D foi encontrado e definir a velocidade da Cruz A
                if (cruzARigidbody != null)
                {
                    cruzARigidbody.velocity = transform.right * cruzASpeed;

                    // Chamar a função do PlayerController para subtrair 1 coração
                    playerController.SubtractHeart();

                    // Iniciar a rotina para fazer o boomerang retornar após 1 segundo
                    StartCoroutine(ReturnBoomerang(cruzAInstance));
                }
            }
        }
    }

    // Função para fazer o boomerang retornar
    private IEnumerator ReturnBoomerang(GameObject cruzAInstance)
    {
        // Aguardar um segundo antes de fazer o boomerang retornar
        yield return new WaitForSeconds(1f);

        // Verificar se o objeto ainda existe antes de acessar o Rigidbody2D
        if (cruzAInstance != null)
        {
            Rigidbody2D cruzARigidbody = cruzAInstance.GetComponent<Rigidbody2D>();

            // Verificar se o Rigidbody2D foi encontrado e fazer o boomerang retornar
            if (cruzARigidbody != null)
            {
                cruzARigidbody.velocity = -cruzARigidbody.velocity; // Inverter a velocidade para retornar

                // Acessar o objeto filho "Brilho Cruz" através do prefab "Cruz A"
                Transform brilhoCruz = cruzAInstance.transform.Find("Brilho Cruz");

                // Verificar se o objeto filho foi encontrado e modificar sua posição em X
                if (brilhoCruz != null)
                {
                    brilhoCruz.localPosition = new Vector3(0.21f, brilhoCruz.localPosition.y, brilhoCruz.localPosition.z);
                }
            }
        }
    }
}
