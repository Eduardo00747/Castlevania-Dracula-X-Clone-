using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelaItens : MonoBehaviour
{
    public GameObject SubItem; // Prefab do objeto "Cora��o"

    private void OnDestroy()
    {
        // Verificar se a vela foi destru�da
        if (gameObject.CompareTag("Vela"))
        {
            // Instanciar o objeto "Cora��o Pequeno" na mesma posi��o da vela
            Instantiate(SubItem, transform.position, Quaternion.identity);
        }
    }
}