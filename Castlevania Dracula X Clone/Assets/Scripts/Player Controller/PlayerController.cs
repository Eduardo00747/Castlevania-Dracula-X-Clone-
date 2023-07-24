using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Variáveis necessárias para a mecânica do personagem
    public float speed = 5f; // Velocidade de movimento
    public float jumpForce = 5f; // Força do pulo
    public float knockbackForce = 5f; // Força do knockback
    public float knockbackDuration = 0.5f; // Duração do knockback

    private bool isJumping = false; // Verifica se o personagem está pulando
    private bool isCrouching = false; // Verifica se o personagem está agachado
    private bool isAlert = false; // Verifica se o personagem está em estado de alerta
    private bool isAttacking = false; // Verifica se o personagem está atacando
    private bool canFlip = true; // Verifica se o personagem pode fazer flip

    // Movimentos de armas
    private bool isThrowingWeapon = false; // Verifica se o personagem está jogando a arma
    private bool isAttackingDown = false; // Verifica se o personagem está realizando um ataque abaixado

    // Movimentos especiais
    private bool isEspecialInicio = false; // Verifica se o personagem está iniciando o especial
    private float especialDuration = 0.5f; // Duração total da animação "Especial Inicio" em segundos

    private bool isEspecialFim = false; // Verifica se o personagem está no estado de "Especial Fim"
    private bool isUsingSpecial = false; // Verifica se o personagem está executando a animação especial

    private bool canMoveDuringSpecial = true; // Verifica se o personagem pode se mover durante a animação especial
    private bool isSpecialAnimationFinished = true; // Verifica se a animação especial terminou

    // Variação para os objetos filhos
    public GameObject hitBoxAtaque; // Referência ao objeto HitBoxAtaque
    public GameObject dropItem; // Referência ao objeto Drop Item
    public GameObject throwCruz; // Referência ao objeto Throw Cruz

    // Variáveis adicionais
    private float hitBoxOffsetX = 0.811f; // Posição X inicial da HitBoxAtaque
    private float dropItemOffsetX = 0.44f; // Posição X inicial do Drop Item
    private bool isMovingLeft = false; // Verifica se o personagem está se movendo para a esquerda

    // Corpo e sprite do objeto do personagem
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Contagem de coração
    public TMP_Text contagemCoracoesText; // Referência para o objeto de texto "Contagem Corações"
    public int coracoesColetados = 10; // Contagem de corações coletados
    private bool canMoveHorizontally = true; // Controla se o personagem pode se mover horizontalmente

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Inicializar o objeto de texto com a contagem de corações
        contagemCoracoesText.text = "// 10";

        // Obtém a referência do objeto "Throw Cruz"
        throwCruz = transform.Find("Throw Cruz").gameObject;
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
        if (!isCrouching && !isAlert && !isAttacking && canMoveHorizontally && !isUsingSpecial)
        {
            // Normalizar o vetor de movimento para manter a velocidade constante
            movement = movement.normalized;

            // Aplicar a força de movimento ao Rigidbody
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);

            // Verificar se o jogador está se movendo para a esquerda e inverter o sprite
            if (horizontalInput < 0 && !isJumping && canFlip)
            {
                spriteRenderer.flipX = true;
            }
            // Verificar se o jogador está se movendo para a direita e restaurar a orientação do sprite
            else if (horizontalInput > 0 && !isJumping && canFlip)
            {
                spriteRenderer.flipX = false;
            }
        }
        else if (horizontalInput < 0 && !isJumping && canFlip)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0 && !isJumping && canFlip)
        {
            spriteRenderer.flipX = false;
        }

        // Verificar se o jogador está no chão e pressionou o botão de espaço para pular
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isCrouching && !isAlert && !isAttacking && !isUsingSpecial)
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
        if (Mathf.Abs(horizontalInput) > 0f && !isCrouching && !isAlert && !isAttacking && canMoveHorizontally && !isUsingSpecial)
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

        // Verificar se o jogador está pressionando "Vertical Cima" e "K" para jogar a arma
        if (verticalInput > 0 && Input.GetKeyDown(KeyCode.K) && !isThrowingWeapon)
        {
            isThrowingWeapon = true;
            animator.SetBool("ThrowWeapon", true);
            // Coloque aqui o código adicional necessário quando o jogador joga a arma
        }

        // Verificar se o jogador está pressionando "Vertical Baixo" e "K" para realizar o ataque abaixado
        if (verticalInput < 0 && Input.GetKeyDown(KeyCode.K) && !isAttacking && !isAlert && !isUsingSpecial)
        {
            //IniciarEspecial();
            isAttackingDown = true;
            animator.SetBool("AtaqueDown", true);

            // Ativar o objeto HitBoxAtaque após um atraso de 0.08 segundos
            Invoke("ActivateHitBoxAtaque", 0.08f);
        }

        // Verificar se o jogador está pressionando "Vertical Cima" e "P" para iniciar o especial
        if (verticalInput > 0 && Input.GetKeyDown(KeyCode.P) && isSpecialAnimationFinished && !isUsingSpecial && !isJumping)
        {
            // Verificar se o jogador tem pelo menos 15 corações antes de iniciar o especial
            if (coracoesColetados >= 15)
            {
                // Subtrair 15 corações da contagem
                coracoesColetados -= 15;

                // Atualizar o objeto de texto com a nova contagem de corações
                contagemCoracoesText.text = "// " + coracoesColetados.ToString();

                // Iniciar o especial
                IniciarEspecial();
            }
            else
            {
                // Mostrar uma mensagem de que o personagem não tem corações suficientes para o especial
                Debug.Log("Não há corações suficientes para ativar o especial!");
            }
        }

        // Verificar se o jogador pressionou a tecla "K" para iniciar o ataque, mas apenas se não estiver no estado de alerta
        if (Input.GetKeyDown(KeyCode.K) && !isAttacking && !isAlert && !isUsingSpecial)
        {
            isAttacking = true;
            animator.SetBool("Ataque", true);

            // Ativar o objeto HitBoxAtaque após um atraso de 0.08 segundos
            Invoke("ActivateHitBoxAtaque", 0.08f);
        }
        else if (!isAttackingDown)
        {
            animator.SetBool("AtaqueDown", false);
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

        // Atualizar a posição do Throw Cruz com base na direção do movimento
        float throwCruzPosX = isMovingLeft ? -0.353f : 0.353f;
        Vector3 throwCruzPos = throwCruz.transform.localPosition;
        throwCruzPos.x = throwCruzPosX;
        throwCruz.transform.localPosition = throwCruzPos;

        // Verificar se a animação de jogar arma terminou
        if (isThrowingWeapon && animator.GetCurrentAnimatorStateInfo(0).IsName("Jogar arma") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            isThrowingWeapon = false;
            animator.SetBool("ThrowWeapon", false);
        }

        // Verificar se a animação de especial inicio terminou
        if (isEspecialInicio && animator.GetCurrentAnimatorStateInfo(0).IsName("Especial Inicio"))
        {
            // Obter a duração normalizada da animação atual
            float currentNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            // Calcular a velocidade necessária para que a animação dure especialDuration segundos
            float animSpeed = currentNormalizedTime < 1.0f ? 1.0f / currentNormalizedTime : 1.0f;

            // Configurar o parâmetro "Speed" da animação para controlar a velocidade de reprodução
            animator.SetFloat("Speed", animSpeed);

            // Verificar se a animação terminou (quando a duração normalizada for maior ou igual a 1)
            if (currentNormalizedTime >= 1.0f)
            {
                // Chamar o método FinalizarEspecial() após o tempo de duração especificado
                Invoke("FinalizarEspecial", especialDuration);
            }
        }

        // Verificar se a animação de especial fim terminou
        if (isEspecialFim && animator.GetCurrentAnimatorStateInfo(0).IsName("Especial Fim"))
        {
            // Chamar o método para voltar para a animação "Idle" após o tempo especificado
            StartCoroutine(VoltarParaIdleAposEspecialFim());
        }

        // Verificar se o personagem está usando o especial e desativar a movimentação horizontal e o flip
        if (isUsingSpecial && canMoveDuringSpecial)
        {
            canMoveDuringSpecial = false;
            canMoveHorizontally = false;
            canFlip = false;
        }
    }

    // Método para iniciar o especial
    private void IniciarEspecial()
    {
        isUsingSpecial = true;
        isSpecialAnimationFinished = false;
        isEspecialInicio = true;
        animator.SetBool("EspecialInicio", true);
    }

    public void OnSpecialAnimationFinished()
    {
        isSpecialAnimationFinished = true;
    }

    private void FinalizarEspecial()
    {
        isEspecialFim = true;
        OnSpecialAnimationFinished();
        animator.SetBool("EspecialFim", true);

        // Redefinir a variável isEspecialInicio
        isEspecialInicio = false;

        // Atualizar o objeto de texto com a nova contagem de corações
        contagemCoracoesText.text = "// " + coracoesColetados.ToString();
    }

    private IEnumerator VoltarParaIdleAposEspecialFim()
    {
        yield return new WaitForSeconds(1.5f);

        // Redefinir as variáveis isEspecialFim e isUsingSpecial, e desativar a animação EspecialFim
        isEspecialFim = false;
        isUsingSpecial = false;
        animator.SetBool("EspecialFim", false);

        // Verificar se o jogador não está pulando nem agachado para ativar a animação Idle
        if (!isJumping && !isCrouching)
        {
            animator.SetBool("IsWalking", false); // Certificar-se de que a animação "Walk" também seja desativada, se necessário
            animator.SetBool("EspecialInicio", false); // Desativar a animação "EspecialInicio" caso ainda esteja ativa
            animator.SetBool("Idle", true);
        }

        // Reativar a movimentação horizontal e o flip
        canMoveDuringSpecial = true;
        canMoveHorizontally = true;
        canFlip = true;
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

        if (isAttackingDown)
        {
            isAttackingDown = false;
            animator.SetBool("AtaqueDown", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se o personagem colidiu com o chão para permitir o pulo novamente
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;

            // Voltar à animação de "Idle" se não estiver agachado e não estiver em estado de alerta
            if (!isCrouching && !isAlert && !isUsingSpecial)
            {
                animator.SetBool("Jump", false);
            }

            // Desbloquear o movimento horizontal quando tocar o chão
            canMoveHorizontally = true;
        }

        // Linha adicionada para redefinir o parâmetro "Jump" para falso
        animator.SetBool("Jump", false);

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

        // Verificar se o personagem colidiu com um objeto de tag "Inimigo"
        if (collision.gameObject.CompareTag("Inimigo"))
        {
            // Desabilitar a movimentação horizontal
            canMoveHorizontally = false;
            canFlip = false;

            // Ativar animação de dano
            animator.SetBool("isDano", true);

            // Aplicar knockback
            Vector2 knockbackDirection = transform.position - collision.transform.position;
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);

            // Aguardar um tempo antes de permitir a movimentação e o flip novamente
            StartCoroutine(EnableMovementAfterDelay(knockbackDuration));
        }
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMoveHorizontally = true;
        canFlip = true;

        // Desativar animação de dano
        animator.SetBool("isDano", false);
    }

    public void SubtractHeart()
    {
        coracoesColetados--;

        // Garantir que a contagem não seja menor que zero
        coracoesColetados = Mathf.Max(coracoesColetados, 0);

        // Atualizar o objeto de texto com a nova contagem de corações
        contagemCoracoesText.text = "// " + coracoesColetados.ToString();
    }
}