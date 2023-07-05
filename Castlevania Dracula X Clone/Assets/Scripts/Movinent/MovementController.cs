using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 5f; // Velocidade de movimento
    public float jumpForce = 5f; // For�a do pulo
    private bool isJumping = false; // Verifica se o personagem est� pulando
    private bool isCrouching = false; // Verifica se o personagem est� agachado
    private bool isAlert = false; // Verifica se o personagem est� em estado de alerta
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        if (!isCrouching && !isAlert)
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
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isCrouching && !isAlert)
        {
            // Aplicar uma for�a vertical para simular o pulo
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;

            // Ativar a anima��o de pulo
            animator.SetBool("Jump", true);
        }

        // Verificar se o jogador est� se movendo horizontalmente para reproduzir a anima��o "Walk"
        if (Mathf.Abs(horizontalInput) > 0f && !isCrouching && !isAlert)
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
    }
}