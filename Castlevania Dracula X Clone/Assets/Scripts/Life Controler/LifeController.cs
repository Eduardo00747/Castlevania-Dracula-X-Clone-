using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int maxHealth = 100; // Vida m�xima do personagem
    [SerializeField] private int currentHealth; // Vida atual do personagem
    private Animator animator; // Refer�ncia para o componente Animator
    private bool isDead; // Flag para indicar se o personagem est� morto

    //Audio de dano recebido 
    private AudioSource audioSource; // Refer�ncia ao componente AudioSource
    public AudioClip damageSound;
    public AudioClip Morte;

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida atual com o valor m�ximo
        animator = GetComponent<Animator>(); // Obt�m o componente Animator do personagem
        audioSource = GetComponent<AudioSource>(); // Obt�m o componente AudioSource do personagem

    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Reduz a vida atual pelo valor do dano

        // Verifica se a vida chegou a zero ou menos
        if (currentHealth <= 0)
        {
            Die();
            audioSource.PlayOneShot(Morte);
        }
        else
        {
            // Reproduz o efeito sonoro de dano
            audioSource.PlayOneShot(damageSound); // Supondo que voc� definiu um AudioClip chamado "damageSound"
        }
    }

    private void Die()
    {
        // Verifica se o personagem j� est� morto
        if (isDead)
            return;

        // Aqui voc� pode adicionar a l�gica para o que acontece quando o personagem morre,
        // como reiniciar o n�vel, exibir uma mensagem de game over, etc.
        // Por enquanto, vamos apenas desativar o GameObject do personagem.

        // Define o par�metro "Morte" como verdadeiro (true) para ativar a anima��o de morte
        animator.SetBool("Morte", true);

        // Desativa o movimento do personagem
        DisableMovement();

        // Desativa o GameObject do personagem ap�s um tempo para permitir que a anima��o de morte seja reproduzida
        Invoke(nameof(DisableCharacter), 1f);

        Debug.Log("O personagem morreu!");

        // Define a flag de morte como verdadeira
        isDead = true;
    }

    private void DisableMovement()
    {
        // Desativa qualquer controle de movimento do personagem
        // Por exemplo, voc� pode desabilitar scripts de movimento, desabilitar componentes de f�sica, etc.
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
        // Verifica se a colis�o ocorreu com um inimigo de tag "Inimigo"
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            // Reduz 10 pontos de vida
            TakeDamage(10);
        }
    }
}
