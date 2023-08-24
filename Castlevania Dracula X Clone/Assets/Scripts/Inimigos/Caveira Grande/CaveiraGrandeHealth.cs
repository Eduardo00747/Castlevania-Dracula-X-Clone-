using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveiraGrandeHealth : MonoBehaviour
{
    public int maxHealth = 100; // Vida m�xima do inimigo
    private int currentHealth; // Vida atual do inimigo

    // Refer�ncia para o componente Animator do inimigo
    private Animator animator;

    //Audio de dano recebido 
    private AudioSource audioSource; // Refer�ncia ao componente AudioSource
    public AudioClip danoInimigo;

    // Start is called before the first frame update
    void Start()
    {
        // Inicialize a vida atual do inimigo com a vida m�xima
        currentHealth = maxHealth;

        // Obtenha a refer�ncia para o componente Animator do inimigo
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>(); // Obt�m o componente AudioSource do personagem
    }

    // Fun��o para receber dano
    public void TakeDamage(int damageAmount)
    {
        // Reduza a vida do inimigo de acordo com o dano recebido
        currentHealth -= damageAmount;

        // Verifique se a vida do inimigo chegou a zero ou menos
        if (currentHealth <= 0)
        {
            // Se a vida chegou a zero ou menos, chame a fun��o de morte
            Die();
            audioSource.PlayOneShot(danoInimigo);
        }
    }

    // Fun��o para executar a morte do inimigo
    private void Die()
    {
        // Ative o par�metro "MorteCav" na anima��o para executar a anima��o de morte
        animator.SetBool("MorteCav", true);

        // Desative o collider para que o inimigo n�o possa mais ser atingido ap�s a morte
        //GetComponent<Collider2D>().enabled = false;

        // Desative o script de movimenta��o para que o inimigo pare de se mover ap�s a morte
        // Se o inimigo estiver se movendo por meio de um script de movimenta��o, desative-o aqui
        // Exemplo: GetComponent<MovimentacaoInimigo>().enabled = false;

        // Ap�s um certo tempo (se necess�rio), voc� pode destruir o objeto do inimigo
        // Por exemplo, se quiser que a anima��o de morte seja reproduzida por completo antes da destrui��o, voc� pode usar uma corrotina para adicionar um pequeno atraso antes da destrui��o
        StartCoroutine(DestroyAfterDelay());
    }

    // Corrotina para destruir o objeto do inimigo ap�s um atraso (opcional)
    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1.0f); // Tempo de atraso (por exemplo, 2 segundos)
        Destroy(gameObject); // Destrua o objeto do inimigo ap�s o atraso
    }

    // Fun��o para detectar colis�es
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifique se o inimigo colidiu com a tag "Ataque"
        if (collision.CompareTag("Ataque") || collision.gameObject.CompareTag("Machado") || collision.gameObject.CompareTag("Cruz") || collision.gameObject.CompareTag("Faca"))
        {
            // Se colidiu com a tag "Ataque", chame a fun��o TakeDamage e passe o valor do dano (por exemplo, 10)
            TakeDamage(10);
        }
    }
}
