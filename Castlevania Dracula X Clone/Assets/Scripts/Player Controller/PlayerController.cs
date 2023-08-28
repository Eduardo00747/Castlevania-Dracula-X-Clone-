using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Vari�veis necess�rias para a mec�nica do personagem
    public float speed = 5f; // Velocidade de movimento
    public float jumpForce = 5f; // For�a do pulo
    public float knockbackForce = 5f; // For�a do knockback
    public float knockbackDuration = 0.5f; // Dura��o do knockback
    private bool verticalKeyPressed = false;

    //Variaveis Escada
    private GameObject stairsObject; // Refer�ncia ao objeto "Stairs" que cont�m a escada
    private bool isCollidingWithEscada = false; // Verifica se o personagem est� colidindo com a tag "Escada"

    //Variaveis de Controles
    public bool isJumping = false; // Verifica se o personagem est� pulando
    private bool isCrouching = false; // Verifica se o personagem est� agachado
    private bool isAlert = false; // Verifica se o personagem est� em estado de alerta
    private bool isAttacking = false; // Verifica se o personagem est� atacando
    private bool canFlip = true; // Verifica se o personagem pode fazer flip

    //Audio do personagem 
    public AudioSource audioSource; // Adicione esta vari�vel para acessar o componente AudioSource
    public AudioClip attackSound; // Adicione esta vari�vel para armazenar o som de ataque
    public AudioClip soundEspecial;
    public AudioClip coracaoSound;

    // Movimentos de armas
    private bool isThrowingWeapon = false; // Verifica se o personagem est� jogando a arma
    private bool isAttackingDown = false; // Verifica se o personagem est� realizando um ataque abaixado

    // Movimentos especiais
    private bool isEspecialInicio = false; // Verifica se o personagem est� iniciando o especial
    private float especialDuration = 0.5f; // Dura��o total da anima��o "Especial Inicio" em segundos

    private bool isEspecialFim = false; // Verifica se o personagem est� no estado de "Especial Fim"
    public bool isUsingSpecial = false; // Verifica se o personagem est� executando a anima��o especial

    private bool canMoveDuringSpecial = true; // Verifica se o personagem pode se mover durante a anima��o especial
    private bool isSpecialAnimationFinished = true; // Verifica se a anima��o especial terminou

    // Vari�veis para controlar o movimento vertical durante o especial
    private bool isRising = false;
    private Vector3 targetPosition;
    private float riseSpeed = 1.0f; // Ajuste a velocidade de subida conforme necess�rio

    // Varia��o para os objetos filhos
    public GameObject hitBoxAtaque; // Refer�ncia ao objeto HitBoxAtaque
    public GameObject dropItem; // Refer�ncia ao objeto Drop Item
    public GameObject throwCruz; // Refer�ncia ao objeto Throw Cruz
    public GameObject throwMachado; //Referencia ao objeto Throw Machado 
    public GameObject throwFaca; //Referencia ao objeto Throw Machado 
    public GameObject throwAguaBenta; //Referencia ao objeto Throw Machado 
    public GameObject StairsMoviment; // Referencia ao objeto StairsMoviment

    // Vari�veis adicionais
    private float hitBoxOffsetX = 0.811f; // Posi��o X inicial da HitBoxAtaque
    private float dropItemOffsetX = 0.44f; // Posi��o X inicial do Drop Item
    private bool isMovingLeft = false; // Verifica se o personagem est� se movendo para a esquerda

    // Corpo e sprite do objeto do personagem
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float originalGravityScale; // Adicione isso na se��o de vari�veis

    // Contagem de cora��o
    public TMP_Text contagemCoracoesText; // Refer�ncia para o objeto de texto "Contagem Cora��es"
    public int coracoesColetados = 10; // Contagem de cora��es coletados
    public bool canMoveHorizontally = true; // Controla se o personagem pode se mover horizontalmente

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalGravityScale = rb.gravityScale;
        audioSource = GetComponent<AudioSource>(); // Obtenha a refer�ncia do componente AudioSource

        // Inicializar o objeto de texto com a contagem de cora��es
        contagemCoracoesText.text = "// 10";

        // Obter a refer�ncia do objeto "Stairs"
        stairsObject = GameObject.Find("Stairs");

        // Obt�m a refer�ncia do objeto "Throw Cruz"
        throwCruz = transform.Find("Throw Cruz").gameObject;
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

        // Verificar se a tecla vertical foi pressionada
        verticalKeyPressed = (verticalInput != 0);

        // Verificar se o jogador est� agachado para bloquear o movimento
        if (!isCrouching && !isAlert && !isAttacking && canMoveHorizontally && !isUsingSpecial)
        {
            // Normalizar o vetor de movimento para manter a velocidade constante
            movement = movement.normalized;

            // Aplicar a for�a de movimento ao Rigidbody
            rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);

            // Verificar se o jogador est� se movendo para a esquerda e inverter o sprite
            if (horizontalInput < 0 && !isJumping && canFlip)
            {
                spriteRenderer.flipX = true;
            }
            // Verificar se o jogador est� se movendo para a direita e restaurar a orienta��o do sprite
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

        // Verificar se o jogador est� no ch�o e pressionou o bot�o de espa�o para pular
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isCrouching && !isAlert && !isAttacking && !isUsingSpecial)
        {

            // Desativar os objetos filhos do objeto "Stairs"
            DeactivateStairsChildren();

            // Aplicar uma for�a vertical para simular o pulo
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;

            // Ativar a anima��o de pulo
            animator.SetBool("Jump", true);

            // Bloquear o movimento horizontal durante o pulo
            canMoveHorizontally = false;
        }

        // Verificar se o jogador est� se movendo horizontalmente para reproduzir a anima��o "Walk"
        if (Mathf.Abs(horizontalInput) > 0f && !isCrouching && !isAlert && !isAttacking && canMoveHorizontally && !isUsingSpecial)
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

        // Verificar se a tecla "Vertical Cima" est� pressionada para ativar o estado de alerta
        if (verticalInput > 0)
        {
            isAlert = true;
            animator.SetBool("Alerta", true);

            // Ativar todos os filhos do objeto "Stairs"
            ActivateStairsChildren(true);
        }
        else
        {
            isAlert = false;
            animator.SetBool("Alerta", false);

            // Desativar todos os filhos do objeto "Stairs" apenas se n�o estiver colidindo com a tag "Escada"
            if (!isCollidingWithEscada)
            {
                ActivateStairsChildren(false);
            }
        }

        // Verificar se o jogador est� pressionando "Vertical Cima" e "K" para jogar a arma
        if (verticalInput > 0 && Input.GetKeyDown(KeyCode.K) && !isThrowingWeapon)
        {
            isThrowingWeapon = true;
            animator.SetBool("ThrowWeapon", true);
            // Coloque aqui o c�digo adicional necess�rio quando o jogador joga a arma
        }

        // Verificar se o jogador est� pressionando "Vertical Baixo" e "K" para realizar o ataque abaixado
        if (verticalInput < 0 && Input.GetKeyDown(KeyCode.K) && !isAttacking && !isAlert && !isUsingSpecial)
        {
            //IniciarEspecial();
            isAttackingDown = true;
            animator.SetBool("AtaqueDown", true);

            // Ativar o objeto HitBoxAtaque ap�s um atraso de 0.08 segundos
            Invoke("ActivateHitBoxAtaque", 0.08f);
        }

        // Verificar se o jogador est� pressionando "Vertical Cima" e "P" para iniciar o especial
        if (verticalInput > 0 && Input.GetKeyDown(KeyCode.P) && isSpecialAnimationFinished && !isUsingSpecial && !isJumping)
        {
            // Verificar se o jogador tem pelo menos 15 cora��es antes de iniciar o especial
            if (coracoesColetados >= 15)
            {
                // Subtrair 15 cora��es da contagem
                coracoesColetados -= 15;

                // Atualizar o objeto de texto com a nova contagem de cora��es
                contagemCoracoesText.text = "// " + coracoesColetados.ToString();

                // Iniciar o especial
                IniciarEspecial();
            }
            else
            {
                // Mostrar uma mensagem de que o personagem n�o tem cora��es suficientes para o especial
                Debug.Log("N�o h� cora��es suficientes para ativar o especial!");
            }
        }

        // Verificar se o jogador pressionou a tecla "K" para iniciar o ataque, mas apenas se n�o estiver no estado de alerta
        if (Input.GetKeyDown(KeyCode.K) && !isAttacking && !isAlert && !isUsingSpecial)
        {
            isAttacking = true;
            animator.SetBool("Ataque", true);

            // Ativar o objeto HitBoxAtaque ap�s um atraso de 0.08 segundos
            Invoke("ActivateHitBoxAtaque", 0.08f);

            // Reproduzir o som de ataque
            audioSource.PlayOneShot(attackSound);
        }
        else if (!isAttackingDown)
        {
            animator.SetBool("AtaqueDown", false);
        }

        // Verificar a dire��o do movimento horizontal para atualizar a posi��o da HitBoxAtaque
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
        // Atualizar a posi��o da HitBoxAtaque com base na dire��o do movimento
        float hitBoxPosX = isMovingLeft ? -hitBoxOffsetX : hitBoxOffsetX;
        Vector3 hitBoxPos = hitBoxAtaque.transform.localPosition;
        hitBoxPos.x = hitBoxPosX;
        hitBoxAtaque.transform.localPosition = hitBoxPos;

        // Atualizar a posi��o do Drop Item com base na dire��o do movimento
        float dropItemPosX = isMovingLeft ? dropItemOffsetX : -dropItemOffsetX;
        Vector3 dropItemPos = dropItem.transform.localPosition;
        dropItemPos.x = dropItemPosX;
        dropItem.transform.localPosition = dropItemPos;

        // Atualizar a posi��o do Throw Cruz com base na dire��o do movimento
        float throwCruzPosX = isMovingLeft ? -0.353f : 0.353f;
        Vector3 throwCruzPos = throwCruz.transform.localPosition;
        throwCruzPos.x = throwCruzPosX;
        throwCruz.transform.localPosition = throwCruzPos;

        // Atualizar a posi��o do Throw Cruz com base na dire��o do movimento
        float throwMachadoPosX = isMovingLeft ? -0.2f : 0.12f;
        Vector3 throwMachadoPos = throwMachado.transform.localPosition;
        throwMachadoPos.x = throwMachadoPosX;
        throwMachado.transform.localPosition = throwMachadoPos;

        // Atualizar a posi��o do Throw Cruz com base na dire��o do movimento
        float throwFacaPosX = isMovingLeft ? -0.3f : 0.3f;
        Vector3 throwFacaPos = throwFaca.transform.localPosition;
        throwFacaPos.x = throwFacaPosX;
        throwFaca.transform.localPosition = throwFacaPos;

        // Atualizar a posi��o do Throw Cruz com base na dire��o do movimento
        float throwAguaBentaPosX = isMovingLeft ? -0.2f : 0.2f;
        Vector3 throwAguaBentaPos = throwAguaBenta.transform.localPosition;
        throwAguaBentaPos.x = throwAguaBentaPosX;
        throwAguaBenta.transform.localPosition = throwAguaBentaPos;

        // Atualizar a posi��o do StairsMoviment com base na dire��o do movimento
        float StairsMovimentPosX = isMovingLeft ? -0.04f : 0.22f;
        Vector3 StairsMovimentPos = StairsMoviment.transform.localPosition;
        StairsMovimentPos.x = StairsMovimentPosX;
        StairsMoviment.transform.localPosition = StairsMovimentPos;

        // Verificar se a anima��o de jogar arma terminou
        if (isThrowingWeapon && animator.GetCurrentAnimatorStateInfo(0).IsName("Jogar arma") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        {
            isThrowingWeapon = false;
            animator.SetBool("ThrowWeapon", false);
        }

        // Verificar se o personagem est� usando o especial
        if (isUsingSpecial)
        {
            rb.gravityScale = 0f; // Define a gravidade para 0
            if (!isRising) // Verificar se o personagem ainda n�o est� subindo e definir a posi��o alvo
            {
                isRising = true;
                targetPosition = new Vector3(transform.position.x, 1.5f, transform.position.z);
            }

            // Interpolar suavemente entre a posi��o atual e a posi��o alvo
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * riseSpeed);

        }
        else
        {
            rb.gravityScale = originalGravityScale; // Retorna � gravidade original
            isRising = false; // Resetar a flag de subida quando n�o estiver mais usando o especial
        }

        // Verificar se a anima��o de especial inicio terminou
        if (isEspecialInicio && animator.GetCurrentAnimatorStateInfo(0).IsName("Especial Inicio"))
        {
            // Obter a dura��o normalizada da anima��o atual
            float currentNormalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            // Calcular a velocidade necess�ria para que a anima��o dure especialDuration segundos
            float animSpeed = currentNormalizedTime < 1.0f ? 1.0f / currentNormalizedTime : 1.0f;

            // Configurar o par�metro "Speed" da anima��o para controlar a velocidade de reprodu��o
            animator.SetFloat("Speed", animSpeed);

            // Verificar se a anima��o terminou (quando a dura��o normalizada for maior ou igual a 1)
            if (currentNormalizedTime >= 1.0f)
            {
                // Chamar o m�todo FinalizarEspecial() ap�s o tempo de dura��o especificado
                Invoke("FinalizarEspecial", especialDuration);
            }
        }

        // Verificar se a anima��o de especial fim terminou
        if (isEspecialFim && animator.GetCurrentAnimatorStateInfo(0).IsName("Especial Fim"))
        {
            // Chamar o m�todo para voltar para a anima��o "Idle" ap�s o tempo especificado
            StartCoroutine(VoltarParaIdleAposEspecialFim());
        }

        // Verificar se o personagem est� usando o especial e desativar a movimenta��o horizontal e o flip
        if (isUsingSpecial && canMoveDuringSpecial)
        {
            canMoveDuringSpecial = false;
            canMoveHorizontally = false;
            canFlip = false;
        }

        // Verificar se verticalKeyPressed � verdadeiro para fazer o personagem parar imediatamente
        if (verticalKeyPressed && !isJumping && !isCrouching)
        {
            // Parar o personagem imediatamente definindo sua velocidade horizontal para zero
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    // M�todo para iniciar o especial
    private void IniciarEspecial()
    {
        isUsingSpecial = true;
        isSpecialAnimationFinished = false;
        isEspecialInicio = true;
        audioSource.PlayOneShot(soundEspecial);
        animator.SetBool("EspecialInicio", true);
    }

    public void OnSpecialAnimationFinished()
    {
        isSpecialAnimationFinished = true;
    }

    private void ActivateStairsChildren(bool activate)
    {
        for (int i = 0; i < stairsObject.transform.childCount; i++)
        {
            stairsObject.transform.GetChild(i).gameObject.SetActive(activate);
        }
    }

    private void DeactivateStairsChildren()
    {
        for (int i = 0; i < stairsObject.transform.childCount; i++)
        {
            stairsObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void FinalizarEspecial()
    {
        isEspecialFim = true;
        OnSpecialAnimationFinished();
        animator.SetBool("EspecialFim", true);

        // Redefinir a vari�vel isEspecialInicio
        isEspecialInicio = false;

        // Atualizar o objeto de texto com a nova contagem de cora��es
        contagemCoracoesText.text = "// " + coracoesColetados.ToString();
    }

    private IEnumerator VoltarParaIdleAposEspecialFim()
    {
        yield return new WaitForSeconds(1.5f);

        // Redefinir as vari�veis isEspecialFim e isUsingSpecial, e desativar a anima��o EspecialFim
        isEspecialFim = false;
        isUsingSpecial = false;
        animator.SetBool("EspecialFim", false);

        // Verificar se o jogador n�o est� pulando nem agachado para ativar a anima��o Idle
        if (!isJumping && !isCrouching)
        {
            animator.SetBool("IsWalking", false); // Certificar-se de que a anima��o "Walk" tamb�m seja desativada, se necess�rio
            animator.SetBool("EspecialInicio", false); // Desativar a anima��o "EspecialInicio" caso ainda esteja ativa
            animator.SetBool("Idle", true);
        }

        // Reativar a movimenta��o horizontal e o flip
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
        // Verificar se o personagem colidiu com o ch�o para permitir o pulo novamente
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;

            // Voltar � anima��o de "Idle" se n�o estiver agachado e n�o estiver em estado de alerta
            if (!isCrouching && !isAlert && !isUsingSpecial)
            {
                animator.SetBool("Jump", false);
            }

            // Desbloquear o movimento horizontal quando tocar o ch�o
            canMoveHorizontally = true;
        }

        // Linha adicionada para redefinir o par�metro "Jump" para falso
        animator.SetBool("Jump", false);

        // Verificar se o personagem colidiu com um objeto de tag "Coracao"
        if (collision.gameObject.CompareTag("Coracao"))
        {
            //Audio ao pegar cora��es 
            audioSource.PlayOneShot(coracaoSound);

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
            //Audio ao pegar cora��es 
            audioSource.PlayOneShot(coracaoSound);

            // Destruir o objeto "Cora��o Grande"
            Destroy(collision.gameObject);

            // Subtrair 10 da contagem de cora��es coletados
            coracoesColetados += 10;

            // Garantir que a contagem n�o seja menor que zero
            coracoesColetados = Mathf.Max(coracoesColetados, 0);

            // Atualizar o objeto de texto com a nova contagem de cora��es
            contagemCoracoesText.text = "// " + coracoesColetados.ToString();
        }

        // Verificar se o personagem colidiu com um objeto de tag "Escada"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isCollidingWithEscada = true;
        }

        // Verificar se o personagem colidiu com um objeto de tag "Inimigo"
        if (collision.gameObject.CompareTag("Inimigo")||collision.gameObject.CompareTag("Morcego"))
        {
            // Desabilitar a movimenta��o horizontal
            canMoveHorizontally = false;
            canFlip = false;

            // Ativar anima��o de dano
            animator.SetBool("isDano", true);

            // Aplicar knockback
            Vector2 knockbackDirection = transform.position - collision.transform.position;
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);

            // Aguardar um tempo antes de permitir a movimenta��o e o flip novamente
            StartCoroutine(EnableMovementAfterDelay(knockbackDuration));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        // Verificar se o personagem saiu do colisor de um objeto de tag "Escada"
        if (other.gameObject.CompareTag("EscadaOff"))
        {
            isCollidingWithEscada = false;

            // Desativar todos os filhos do objeto "Stairs" apenas se n�o estiver pressionando a tecla "Vertical Cima"
            if (!isAlert)
            {
                ActivateStairsChildren(false);
                DeactivateStairsChildren();
            }
        }
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMoveHorizontally = true;
        canFlip = true;

        // Desativar anima��o de dano
        animator.SetBool("isDano", false);
    }

    public void SubtractHeart()
    {
        coracoesColetados--;

        // Garantir que a contagem n�o seja menor que zero
        coracoesColetados = Mathf.Max(coracoesColetados, 0);

        // Atualizar o objeto de texto com a nova contagem de cora��es
        contagemCoracoesText.text = "// " + coracoesColetados.ToString();
    }
}