using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelaItens : MonoBehaviour
{
    public GameObject SubItem; // Prefab do objeto "Coração"

    private void OnDestroy()
    {
        // Verificar se a vela foi destruída
        if (gameObject.CompareTag("Vela"))
        {
            // Instanciar o objeto "Coração Pequeno" na mesma posição da vela
            Instantiate(SubItem, transform.position, Quaternion.identity);
        }
    }
}