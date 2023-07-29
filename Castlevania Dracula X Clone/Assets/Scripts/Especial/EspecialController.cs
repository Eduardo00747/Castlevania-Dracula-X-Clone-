using System.Collections;
using UnityEngine;

public class EspecialController : MonoBehaviour
{
    // Referência ao componente Rigidbody2D do personagem
    private Rigidbody2D rb;

    // Referência ao componente Animator do personagem
    private Animator animator;

    // Variável para armazenar a posição original do personagem em Y
    private float originalYPosition;

    // Velocidade de subida e descida durante o especial
    public float especialAscendSpeed = 3f;
    public float especialDescendSpeed = 5f;

    // Altura máxima de ascensão durante o especial
    public float maxAscendHeight = 1.5f;

    // Tempo de duração do especial (ascensão + queda)
    public float especialDuration = 2f;

    // Tag para identificar o chão
    public string groundTag = "Ground";

    // Variável para verificar se o personagem está executando o especial
    private bool isUsingEspecial = false;

    // Tempo de espera (cooldown) após executar o especial antes de poder executá-lo novamente
    public float especialCooldownTime = 3f;
    private bool canUseEspecial = true;

    private void Start()
    {
        // Obter as referências dos componentes Rigidbody2D e Animator
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Verificar se o jogador está pressionando "Vertical Cima" + "P" para iniciar o especial
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.P) && canUseEspecial && !isUsingEspecial && !GetComponent<PlayerController>().isJumping)
        {
            // Iniciar o especial
            StartCoroutine(ExecuteEspecial());

            // Iniciar o cooldown
            StartCoroutine(StartEspecialCooldown());
        }

        // Verificar se o personagem está executando o especial
        if (isUsingEspecial)
        {
            // Verificar se o personagem ainda está subindo
            if (transform.position.y - originalYPosition < maxAscendHeight)
            {
                // Movimentar o personagem para cima
                rb.velocity = new Vector2(rb.velocity.x, especialAscendSpeed);
            }
            else
            {
                // Parar o movimento vertical
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
        }
    }

    private IEnumerator StartEspecialCooldown()
    {
        // Desativar o uso do especial
        canUseEspecial = false;

        // Aguardar o tempo de cooldown
        yield return new WaitForSeconds(especialCooldownTime);

        // Permitir o uso do especial novamente após o cooldown
        canUseEspecial = true;
    }

    private IEnumerator ExecuteEspecial()
    {
        // Bloquear o movimento horizontal e o pulo durante o especial
        GetComponent<PlayerController>().canMoveHorizontally = false;
        GetComponent<PlayerController>().isJumping = false;

        // Armazenar a posição original do personagem em Y
        originalYPosition = transform.position.y;

        // Ativar a animação de especial
        isUsingEspecial = true;
        animator.SetBool("EspecialInicio", true);

        // Aguardar a duração do especial
        yield return new WaitForSeconds(especialDuration);

        // Desativar a animação de especial e permitir a movimentação horizontal novamente
        animator.SetBool("EspecialFim", false);
        GetComponent<PlayerController>().canMoveHorizontally = true;

        // Iniciar a animação de queda
        rb.velocity = new Vector2(rb.velocity.x, -especialDescendSpeed);

        // Aguardar a conclusão da animação de queda antes de reativar o pulo
        yield return new WaitForSeconds(especialDuration);

        // Verificar se o personagem colidiu com o chão
        if (IsGrounded())
        {
            GetComponent<PlayerController>().isJumping = false;
        }

        // Permitir o pulo novamente
        GetComponent<PlayerController>().canMoveHorizontally = true;

        // Finalizar o especial
        isUsingEspecial = false;
    }

    private bool IsGrounded()
    {
        // Verificar se o personagem está tocando algum objeto com a tag "Ground"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(groundTag))
            {
                return true;
            }
        }

        return false;
    }
}