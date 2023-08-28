using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vida máxima do inimigo
    private int currentHealth; // Vida atual do inimigo

    public GameObject esqueletoPequeno; // Referência para o objeto "Esqueleto Pequeno"

    public GameObject cabecaPrefab;
    public GameObject troncoPrefab;
    public GameObject bracoPrefab;
    public GameObject pernaPrefab;

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
        // Destruir o objeto "Esqueleto Pequeno"
        Destroy(esqueletoPequeno);
        Debug.Log("O inimigo morreu!");

        // Ativar e soltar os objetos filhos
        Instantiate(cabecaPrefab, transform.position, Quaternion.identity);
        Instantiate(troncoPrefab, transform.position, Quaternion.identity);
        Instantiate(bracoPrefab, transform.position, Quaternion.identity);
        Instantiate(pernaPrefab, transform.position, Quaternion.identity);

        // Aqui você pode adicionar a lógica para o que acontece quando o inimigo morre,
        // como dar pontos ao jogador, reproduzir uma animação de morte, etc.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se a colisão ocorreu com o jogador de tag "Player"
        if (collision.gameObject.CompareTag("Ataque"))
        {
            // Reduz 10 pontos de vida
            TakeDamage(10);
        }
    }

    // Função para detectar colisões
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifique se o inimigo colidiu com a tag "Ataque"
        if (collision.gameObject.CompareTag("Machado") || collision.gameObject.CompareTag("Cruz") || collision.gameObject.CompareTag("Faca"))
        {
            // Se colidiu com a tag "Ataque", chame a função TakeDamage e passe o valor do dano (por exemplo, 10)
            TakeDamage(10);
        }
    }
}
