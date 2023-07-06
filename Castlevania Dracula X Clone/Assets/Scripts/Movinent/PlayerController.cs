using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Velocidade de movimento
    public float jumpForce = 5f; // For�a do pulo
    private bool isJumping = false; // Verifica se o personagem est� pulando
    private bool isCrouching = false; // Verifica se o personagem est� agachado
    private bool isAlert = false; // Verifica se o personagem est� em estado de alerta
    private bool isAttacking = false; // Verifica se o personagem est� atacando
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public TMP_Text contagemCoracoesText; // Refer�ncia para o objeto de texto "Contagem Cora��es"

    private int coracoesColetados = 0; // Contagem de cora��es coletados


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Inicializar o objeto de texto com a contagem de cora��es
        contagemCoracoesText.text = "// 0";
    }

    private void Update()
    {
        // Obter entrada do teclado
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Verificar se o jogador est� agachado para bloquear o movimento e o pulo
        if (verticalInput < 0 && !isCrouching)
        {
            isCrouching = true;
            animator.SetBool("Crouch", true);

            // Zerar o movimento horizontal e impedir o pulo
            horizontalInput = 0f;
            isJumping = false;
        }
        else if (verticalInput >= 0 && isCrouching)
        {
            isCrouching = false;
            animator.SetBool("Crouch", false);
        }

        // Calcular a dire��o do movimento
        Vector2 movement = new Vector2(horizontalInput, 0f);

        // Verificar se o jogador est� agachado para bloquear o movimento
        if (!isCrouching && !isAlert && !isAttacking)
        {
            // Normalizar o vetor de movimento para manter a velocidade constante
            movement = movement.normalized;

            // Aplicar a for�a de movimento ao Rigidbody
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);

            // Verificar se o jogador est� se movendo para a esquerda e inverter o sprite
            if (horizontalInput < 0)
            {
                spriteRenderer.flipX = true;
            }
            // Verificar se o jogador est� se movendo para a direita e restaurar a orienta��o do sprite
            else if (horizontalInput > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }

        // Verificar se o jogador est� no ch�o e pressionou o bot�o de espa�o para pular
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isCrouching && !isAlert && !isAttacking)
        {
            // Aplicar uma for�a vertical para simular o pulo
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;

            // Ativar a anima��o de pulo
            animator.SetBool("Jump", true);
        }

        // Verificar se o jogador est� se movendo horizontalmente para reproduzir a anima��o "Walk"
        if (Mathf.Abs(horizontalInput) > 0f && !isCrouching && !isAlert && !isAttacking)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        // Verificar se a tecla "Vertical Cima" est� pressionada para ativar o estado de alerta
        if (verticalInput > 0)
        {
            isAlert = true;
            animator.SetBool("Alerta", true);
        }
        else
        {
            isAlert = false;
            animator.SetBool("Alerta", false);
        }

        // Verificar se o jogador pressionou a tecla "K" para iniciar o ataque
        if (Input.GetKeyDown(KeyCode.K) && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("Ataque", true);
        }

    }

    public void EndAttackAnimation()
    {
        isAttacking = false;
        animator.SetBool("Ataque", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se o personagem colidiu com o ch�o para permitir o pulo novamente
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;

            // Voltar � anima��o de "Idle" se n�o estiver agachado e n�o estiver em estado de alerta
            if (!isCrouching && !isAlert)
            {
                animator.SetBool("Jump", false);
            }
        }

        // Verificar se o personagem colidiu com um objeto de tag "Coracao"
        if (collision.gameObject.CompareTag("Coracao"))
        {
            // Destruir o objeto "Cora��o Pequeno"
            Destroy(collision.gameObject);

            // Incrementar a contagem de cora��es coletados
            coracoesColetados++;

            // Atualizar o objeto de texto com a nova contagem de cora��es
            contagemCoracoesText.text = "// " + coracoesColetados.ToString();
        }

        // Verificar se o personagem colidiu com um objeto de tag "Coracao Grande"
        if (collision.gameObject.CompareTag("Coracao Grande"))
        {
            // Destruir o objeto "Cora��o Grande"
            Destroy(collision.gameObject);

            // Subtrair 10 da contagem de cora��es coletados
            coracoesColetados += 10;

            // Garantir que a contagem n�o seja menor que zero
            coracoesColetados = Mathf.Max(coracoesColetados, 0);

            // Atualizar o objeto de texto com a nova contagem de cora��es
            contagemCoracoesText.text = "// " + coracoesColetados.ToString();
        }
    }
}