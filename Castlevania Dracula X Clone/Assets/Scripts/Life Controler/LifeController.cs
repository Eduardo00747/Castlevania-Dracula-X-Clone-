using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int maxHealth = 100; // Vida m�xima do personagem
    [SerializeField] private int currentHealth; // Vida atual do personagem

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida atual com o valor m�ximo
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
        // Aqui voc� pode adicionar a l�gica para o que acontece quando o personagem morre,
        // como reiniciar o n�vel, exibir uma mensagem de game over, etc.
        // Por enquanto, vamos apenas desativar o GameObject do personagem.
        gameObject.SetActive(false);
        Debug.Log("O personagem morreu!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colis�o ocorreu com um inimigo de tag "Inimigo"
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            // Reduz 10 pontos de vida
            TakeDamage(10);

            // Destroi o proj�til
            Destroy(collision.gameObject);
        }
    }
}