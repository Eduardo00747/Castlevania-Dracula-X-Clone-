using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCruz : MonoBehaviour
{
    public GameObject cruzAPrefab; // Prefab da Cruz A
    public float cruzASpeed = 10f; // Velocidade da Cruz A

    private bool isReturning = false; // Controla se a Cruz A está retornando
    private float returnTimer = 1f; // Tempo para a Cruz A iniciar o retorno (em segundos)

    // Update is called once per frame
    void Update()
    {
        // Verificar se o jogador está pressionando "K" para atirar a Cruz A
        if (Input.GetKeyDown(KeyCode.K))
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
            }
        }
    }
}