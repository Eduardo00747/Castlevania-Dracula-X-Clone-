using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Variaveis nescessarias para a mecaninca do personagem
    public float speed = 5f; // Velocidade de movimento
    public float jumpForce = 5f; // Força do pulo
    private bool isJumping = false; // Verifica se o personagem está pulando
    private bool isCrouching = false; // Verifica se o personagem está agachado
    private bool isAlert = false; // Verifica se o personagem está em estado de alerta
    private bool isAttacking = false; // Verifica se o personagem está atacando

    //Variação ´para os objetos filhos
    public GameObject hitBoxAtaque; // Referência ao objeto HitBoxAtaque
    public GameObject dropItem; // Referência ao objeto Drop Item

    // Variáveis adicionais
    private float hitBoxOffsetX = 0.811f; // Posição X inicial da HitBoxAtaque
    private float dropItemOffsetX = 0.44f; // Posição X inicial do Drop Item
    private bool isMovingLeft = false; // Verifica se o personagem está se movendo para a esquerda

    //Corpo e sprite do objeto do personagem 
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    //Contagem de coração
    public TMP_Text contagemCoracoesText; // Referência para o objeto de texto "Contagem Corações"
    private int coracoesColetados = 0; // Contagem de corações coletados
    private bool canMoveHorizontally = true; // Controla se o personagem pode se mover horizontalmente

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Inicializar o objeto de texto com a contagem de corações
        contagemCoracoesText.text = "// 0";
    }

    private void Update()
    {
        // Obter entrada do teclado
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Verificar se o jogador está agachado para bloquear o movimento e o pulo
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

        // Calcular a direção do movimento
        Vector2 movement = new Vector2(horizontalInput, 0f);

        // Verificar se o jogador está agachado para bloquear o movimento
        if (!isCrouching && !isAlert && !isAttacking && canMoveHorizontally)
        {
            // Normalizar o vetor de movimento para manter a velocidade constante
            movement = movement.normalized;

            // Aplicar a força de movimento ao Rigidbody
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);

            // Verificar se o jogador está se movendo para a esquerda e inverter o sprite
            if (horizontalInput < 0 && !isJumping)
            {
                spriteRenderer.flipX = true;
            }
            // Verificar se o jogador está se movendo para a direita e restaurar a orientação do sprite
            else if (horizontalInput > 0 && !isJumping)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (horizontalInput < 0 && !isJumping)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0 && !isJumping)
        {
            spriteRenderer.flipX = false;
        }

        // Verificar se o jogador está no chão e pressionou o botão de espaço para pular
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isCrouching && !isAlert && !isAttacking)
        {
            // Aplicar uma força vertical para simular o pulo
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;

            // Ativar a animação de pulo
            animator.SetBool("Jump", true);

            // Bloquear o movimento horizontal durante o pulo
            canMoveHorizontally = false;
        }

        // Verificar se o jogador está se movendo horizontalmente para reproduzir a animação "Walk"
        if (Mathf.Abs(horizontalInput) > 0f && !isCrouching && !isAlert && !isAttacking && canMoveHorizontally)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        // Verificar se a tecla "Vertical Cima" está pressionada para ativar o estado de alerta
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

            // Ativar o objeto HitBoxAtaque após um atraso de 0.08 segundos
            Invoke("ActivateHitBoxAtaque", 0.08f);
        }

        // Verificar a direção do movimento horizontal para atualizar a posição da HitBoxAtaque
        if (horizontalInput < 0)
        {
            isMovingLeft = true;
        }
        else if (horizontalInput > 0)
        {
            isMovingLeft = false;
        }
    }

    private void LateUpdate()
    {
        // Atualizar a posição da HitBoxAtaque com base na direção do movimento
        float hitBoxPosX = isMovingLeft ? -hitBoxOffsetX : hitBoxOffsetX;
        Vector3 hitBoxPos = hitBoxAtaque.transform.localPosition;
        hitBoxPos.x = hitBoxPosX;
        hitBoxAtaque.transform.localPosition = hitBoxPos;

        // Atualizar a posição do Drop Item com base na direção do movimento
        float dropItemPosX = isMovingLeft ? dropItemOffsetX : -dropItemOffsetX;
        Vector3 dropItemPos = dropItem.transform.localPosition;
        dropItemPos.x = dropItemPosX;
        dropItem.transform.localPosition = dropItemPos;
    }

    private void ActivateHitBoxAtaque()
    {
        // Ativar o objeto HitBoxAtaque
        hitBoxAtaque.SetActive(true);
    }

    public void EndAttackAnimation()
    {
        isAttacking = false;
        animator.SetBool("Ataque", false);

        // Desativar o objeto HitBoxAtaque
        hitBoxAtaque.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se o personagem colidiu com o chão para permitir o pulo novamente
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;

            // Voltar à animação de "Idle" se não estiver agachado e não estiver em estado de alerta
            if (!isCrouching && !isAlert)
            {
                animator.SetBool("Jump", false);
            }

            // Desbloquear o movimento horizontal quando tocar o chão
            canMoveHorizontally = true;
        }

        // Verificar se o personagem colidiu com um objeto de tag "Coracao"
        if (collision.gameObject.CompareTag("Coracao"))
        {
            // Destruir o objeto "Coração Pequeno"
            Destroy(collision.gameObject);

            // Incrementar a contagem de corações coletados
            coracoesColetados++;

            // Atualizar o objeto de texto com a nova contagem de corações
            contagemCoracoesText.text = "// " + coracoesColetados.ToString();
        }

        // Verificar se o personagem colidiu com um objeto de tag "Coracao Grande"
        if (collision.gameObject.CompareTag("Coracao Grande"))
        {
            // Destruir o objeto "Coração Grande"
            Destroy(collision.gameObject);

            // Subtrair 10 da contagem de corações coletados
            coracoesColetados += 10;

            // Garantir que a contagem não seja menor que zero
            coracoesColetados = Mathf.Max(coracoesColetados, 0);

            // Atualizar o objeto de texto com a nova contagem de corações
            contagemCoracoesText.text = "// " + coracoesColetados.ToString();
        }
    }
}