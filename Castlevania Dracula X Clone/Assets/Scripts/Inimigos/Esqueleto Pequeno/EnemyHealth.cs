using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vida máxima do inimigo
    private int currentHealth; // Vida atual do inimigo

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida atual com o valor máximo
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Reduz a vida atual pelo valor do dano

        // Verifica se a vida chegou a zero ou menos
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Aqui você pode adicionar a lógica para o que acontece quando o inimigo morre,
        // como dar pontos ao jogador, reproduzir uma animação de morte, etc.
        // Por enquanto, vamos apenas desativar o GameObject do inimigo.
        gameObject.SetActive(false);
        Debug.Log("O inimigo morreu!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colisão ocorreu com o jogador de tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Reduz 10 pontos de vida
            TakeDamage(10);
        }
    }
}
