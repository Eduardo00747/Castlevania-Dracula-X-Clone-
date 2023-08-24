using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveiraGrandeHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vida máxima do inimigo
    private int currentHealth; // Vida atual do inimigo

    // Referência para o componente Animator do inimigo
    private Animator animator;

    //Audio de dano recebido 
    private AudioSource audioSource; // Referência ao componente AudioSource
    public AudioClip danoInimigo;

    // Start is called before the first frame update
    void Start()
    {
        // Inicialize a vida atual do inimigo com a vida máxima
        currentHealth = maxHealth;

        // Obtenha a referência para o componente Animator do inimigo
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>(); // Obtém o componente AudioSource do personagem
    }

    // Função para receber dano
    public void TakeDamage(int damageAmount)
    {
        // Reduza a vida do inimigo de acordo com o dano recebido
        currentHealth -= damageAmount;

        // Verifique se a vida do inimigo chegou a zero ou menos
        if (currentHealth <= 0)
        {
            // Se a vida chegou a zero ou menos, chame a função de morte
            Die();
            audioSource.PlayOneShot(danoInimigo);
        }
    }

    // Função para executar a morte do inimigo
    private void Die()
    {
        // Ative o parâmetro "MorteCav" na animação para executar a animação de morte
        animator.SetBool("MorteCav", true);

        // Desative o collider para que o inimigo não possa mais ser atingido após a morte
        //GetComponent<Collider2D>().enabled = false;

        // Desative o script de movimentação para que o inimigo pare de se mover após a morte
        // Se o inimigo estiver se movendo por meio de um script de movimentação, desative-o aqui
        // Exemplo: GetComponent<MovimentacaoInimigo>().enabled = false;

        // Após um certo tempo (se necessário), você pode destruir o objeto do inimigo
        // Por exemplo, se quiser que a animação de morte seja reproduzida por completo antes da destruição, você pode usar uma corrotina para adicionar um pequeno atraso antes da destruição
        StartCoroutine(DestroyAfterDelay());
    }

    // Corrotina para destruir o objeto do inimigo após um atraso (opcional)
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // Tempo de atraso (por exemplo, 2 segundos)
        Destroy(gameObject); // Destrua o objeto do inimigo após o atraso
    }

    // Função para detectar colisões
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifique se o inimigo colidiu com a tag "Ataque"
        if (collision.CompareTag("Ataque") || collision.gameObject.CompareTag("Machado") || collision.gameObject.CompareTag("Cruz") || collision.gameObject.CompareTag("Faca"))
        {
            // Se colidiu com a tag "Ataque", chame a função TakeDamage e passe o valor do dano (por exemplo, 10)
            TakeDamage(10);
        }
    }
}
