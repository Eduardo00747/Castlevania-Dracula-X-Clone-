using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxAtaque : MonoBehaviour
{

    // Referência ao componente AudioSource no objeto do personagem
    private AudioSource audioSource;

    // Sound effect a ser reproduzido quando a vela for destruída
    public AudioClip velaSound;

    private void Start()
    {
        // Obter o componente AudioSource no objeto do personagem
        audioSource = GetComponentInParent<AudioSource>();
    }

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
        if (other.CompareTag("Vela")|| other.CompareTag("Morcego"))
        {
            // Reproduz o sound effect associado à destruição da vela
            if (audioSource != null && velaSound != null)
            {
                audioSource.PlayOneShot(velaSound);
            }
            // Destroi o objeto "Vela"
            Destroy(other.gameObject);
        }
    }
}