using System.Collections;
using UnityEngine;

public class EspecialController : MonoBehaviour
{
    // Refer�ncia ao componente Rigidbody2D do personagem
    private Rigidbody2D rb;

    // Refer�ncia ao componente Animator do personagem
    private Animator animator;

    // Vari�vel para armazenar a posi��o original do personagem em Y
    private float originalYPosition;

    // Velocidade de subida e descida durante o especial
    public float especialAscendSpeed = 3f;
    public float especialDescendSpeed = 5f;

    // Altura m�xima de ascens�o durante o especial
    public float maxAscendHeight = 1.5f;

    // Tempo de dura��o do especial (ascens�o + queda)
    public float especialDuration = 2f;

    // Tag para identificar o ch�o
    public string groundTag = "Ground";

    // Vari�vel para verificar se o personagem est� executando o especial
    private bool isUsingEspecial = false;

    // Tempo de espera (cooldown) ap�s executar o especial antes de poder execut�-lo novamente
    public float especialCooldownTime = 3f;
    private bool canUseEspecial = true;

    private void Start()
    {
        // Obter as refer�ncias dos componentes Rigidbody2D e Animator
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Verificar se o jogador est� pressionando "Vertical Cima" + "P" para iniciar o especial
        if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.P) && canUseEspecial && !isUsingEspecial && !GetComponent<PlayerController>().isJumping)
        {
            // Iniciar o especial
            StartCoroutine(ExecuteEspecial());

            // Iniciar o cooldown
            StartCoroutine(StartEspecialCooldown());
        }

        // Verificar se o personagem est� executando o especial
        if (isUsingEspecial)
        {
            // Verificar se o personagem ainda est� subindo
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

        // Permitir o uso do especial novamente ap�s o cooldown
        canUseEspecial = true;
    }

    private IEnumerator ExecuteEspecial()
    {
        // Bloquear o movimento horizontal e o pulo durante o especial
        GetComponent<PlayerController>().canMoveHorizontally = false;
        GetComponent<PlayerController>().isJumping = false;

        // Armazenar a posi��o original do personagem em Y
        originalYPosition = transform.position.y;

        // Ativar a anima��o de especial
        isUsingEspecial = true;
        animator.SetBool("EspecialInicio", true);

        // Aguardar a dura��o do especial
        yield return new WaitForSeconds(especialDuration);

        // Desativar a anima��o de especial e permitir a movimenta��o horizontal novamente
        animator.SetBool("EspecialFim", false);
        GetComponent<PlayerController>().canMoveHorizontally = true;

        // Iniciar a anima��o de queda
        rb.velocity = new Vector2(rb.velocity.x, -especialDescendSpeed);

        // Aguardar a conclus�o da anima��o de queda antes de reativar o pulo
        yield return new WaitForSeconds(especialDuration);

        // Verificar se o personagem colidiu com o ch�o
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
        // Verificar se o personagem est� tocando algum objeto com a tag "Ground"
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