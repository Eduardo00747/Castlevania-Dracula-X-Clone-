using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int maxHealth = 100; // Vida máxima do personagem
    [SerializeField] private int currentHealth; // Vida atual do personagem
    private Animator animator; // Referência para o componente Animator
    private bool isDead; // Flag para indicar se o personagem está morto

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida atual com o valor máximo
        animator = GetComponent<Animator>(); // Obtém o componente Animator do personagem
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
        // Verifica se o personagem já está morto
        if (isDead)
            return;

        // Aqui você pode adicionar a lógica para o que acontece quando o personagem morre,
        // como reiniciar o nível, exibir uma mensagem de game over, etc.
        // Por enquanto, vamos apenas desativar o GameObject do personagem.

        // Define o parâmetro "Morte" como verdadeiro (true) para ativar a animação de morte
        animator.SetBool("Morte", true);

        // Desativa o movimento do personagem
        DisableMovement();

        // Desativa o GameObject do personagem após um tempo para permitir que a animação de morte seja reproduzida
        Invoke("DisableCharacter", 1f);

        Debug.Log("O personagem morreu!");

        // Define a flag de morte como verdadeira
        isDead = true;
    }

    private void DisableMovement()
    {
        // Desativa qualquer controle de movimento do personagem
        // Por exemplo, você pode desabilitar scripts de movimento, desabilitar componentes de física, etc.
        // Desabilita o script de movimento do personagem
        GetComponent<PlayerController>().enabled = false;
    }

    private void DisableCharacter()
    {
        // Desativa o GameObject do personagem
        gameObject.SetActive(false);
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