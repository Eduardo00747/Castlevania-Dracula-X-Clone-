using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxAtaque : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se colidiu com um inimigo de tag "Enemy"
        if (other.CompareTag("Inimigo"))
        {
            // Obtém o componente EnemyHealth do inimigo
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

            // Verifica se o componente existe
            if (enemyHealth != null)
            {
                // Aplica o dano ao inimigo
                enemyHealth.TakeDamage(10);
            }
        }

        // Verifica se colidiu com um objeto de tag "Vela"
        if (other.CompareTag("Vela"))
        {
            // Destroi o objeto "Vela"
            Destroy(other.gameObject);
        }
    }
}