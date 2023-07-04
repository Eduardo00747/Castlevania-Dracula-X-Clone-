using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int maxHealth = 100; // Vida máxima do personagem
    [SerializeField] private int currentHealth; // Vida atual do personagem

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
        // Aqui você pode adicionar a lógica para o que acontece quando o personagem morre,
        // como reiniciar o nível, exibir uma mensagem de game over, etc.
        // Por enquanto, vamos apenas desativar o GameObject do personagem.
        gameObject.SetActive(false);
        Debug.Log("O personagem morreu!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colisão ocorreu com um inimigo de tag "Inimigo"
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            // Reduz 10 pontos de vida
            TakeDamage(10);

            // Destroi o projétil
            Destroy(collision.gameObject);
        }
    }
}