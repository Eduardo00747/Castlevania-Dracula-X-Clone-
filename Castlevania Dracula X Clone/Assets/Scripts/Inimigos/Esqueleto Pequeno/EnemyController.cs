using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float patrolSpeed = 3f; // Velocidade de patrulha
    public float leftPatrolDuration = 3f; // Duração da patrulha para a esquerda em segundos
    public float rightPatrolDuration = 3f; // Duração da patrulha para a direita em segundos
    public float patrolStopDuration = 1f; // Duração da parada durante a patrulha
    public float chaseSpeed = 5f; // Velocidade de perseguição
    public float chaseRange = 5f; // Alcance de perseguição
    public GameObject projectilePrefab; // Prefab do projétil
    public Transform projectileSpawnPoint; // Ponto de spawn do projétil
    public float projectileSpeed = 10f; // Velocidade do projétil
    private float patrolTimer; // Tempo decorrido da patrulha
    private float stopTimer; // Tempo decorrido da parada
    private int moveDirection = 1; // Direção do movimento: 1 para direita, -1 para esquerda
    private bool hasAttacked; // Verifica se o inimigo já atacou durante a pausa atual
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform spawnOssoTransform; // Referência para o transform do objeto "Spawn Osso"


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Obter a referência do transform do objeto "Spawn Osso"
        spawnOssoTransform = transform.Find("Spawn Osso");

    }

    private void Update()
    {
        // Verificar se o personagem está dentro do alcance de perseguição
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= chaseRange)
        {
            // Calcular a direção do movimento em relação ao personagem
            moveDirection = (int)Mathf.Sign(player.transform.position.x - transform.position.x);

            bool isPlayerOnRight = player.transform.position.x > transform.position.x;

            if (isPlayerOnRight)
            {
                spriteRenderer.flipX = true; // Inverte o sprite em X
            }
            else
            {
                spriteRenderer.flipX = false; // Restaura a orientação do sprite
            }

            if (isPlayerOnRight)
            {
                spriteRenderer.flipX = true; // Inverte o sprite em X

                // Verificar se o objeto "Spawn Osso" existe e ajustar sua posição em X
                if (spawnOssoTransform != null)
                {
                    Vector3 newPosition = spawnOssoTransform.localPosition;
                    newPosition.x = 0.13f;
                    spawnOssoTransform.localPosition = newPosition;
                }
            }
            else
            {
                spriteRenderer.flipX = false; // Restaura a orientação do sprite

                // Verificar se o objeto "Spawn Osso" existe e ajustar sua posição de volta
                if (spawnOssoTransform != null)
                {
                    Vector3 originalPosition = spawnOssoTransform.localPosition;
                    originalPosition.x = -0.17f; // Posição original em X
                    spawnOssoTransform.localPosition = originalPosition;
                }
            }

            // Movimento de perseguição com pausas para ataque do persongem
            float chaseMovement = Mathf.PingPong(Time.time, 2f) <= 0.5f ? 0f : moveDirection * chaseSpeed;
            rb.velocity = new Vector2(chaseMovement, rb.velocity.y);

            // Controlar a animação de ataque ao personagem 
            bool isAttacking = Mathf.PingPong(Time.time, 2f) <= 0.5f;
            animator.SetBool("JogaOsso", isAttacking);

            // Verificar se o inimigo está atacando
            if (isAttacking && !hasAttacked)
            {
                // Atirar o projétil após 0.5 segundos de atraso
                Invoke("ShootProjectile", 0.5f);

                // Definir que o inimigo já atacou durante a pausa atual
                hasAttacked = true;
            }
            else if (!isAttacking)
            {
                // Reiniciar o status de ataque para a próxima pausa
                hasAttacked = false;
            }

            // Reiniciar os timers da patrulha
            patrolTimer = 0f;
            stopTimer = 0f;
        }
        else
        {
            // Movimento de patrulha
            float patrolMovement = moveDirection * patrolSpeed;
            rb.velocity = new Vector2(patrolMovement, rb.velocity.y);

            // Atualizar o tempo da patrulha
            patrolTimer += Time.deltaTime;

            // Verificar se o tempo de parada é atingido
            if (stopTimer >= patrolStopDuration)
            {
                // Verificar se a duração da patrulha para a esquerda foi atingida
                if (moveDirection == -1 && patrolTimer >= leftPatrolDuration)
                {
                    // Parar o movimento
                    rb.velocity = Vector2.zero;

                    // Iniciar a contagem regressiva para a próxima direção
                    stopTimer += Time.deltaTime;
                }
                // Verificar se a duração da patrulha para a direita foi atingida
                else if (moveDirection == 1 && patrolTimer >= rightPatrolDuration)
                {
                    // Parar o movimento
                    rb.velocity = Vector2.zero;

                    // Iniciar a contagem regressiva para a próxima direção
                    stopTimer += Time.deltaTime;
                }
                // Caso contrário, continuar a patrulha normalmente
                else
                {
                    // Reiniciar o timer de parada
                    stopTimer = 0f;
                }
            }
            // Caso contrário, continuar a patrulha normalmente
            else
            {
                // Atualizar o tempo de parada
                stopTimer += Time.deltaTime;
            }

            // Verificar se a duração total da patrulha foi atingida
            if (patrolTimer >= leftPatrolDuration + rightPatrolDuration)
            {
                // Inverter a direção do movimento
                moveDirection *= -1;

                // Reiniciar o timer da patrulha
                patrolTimer = 0f;
            }
        }
    }

    private void ShootProjectile()
    {
        // Instanciar o projétil a partir do prefab
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Adicionar componente de colisão ao projetil
        projectile.AddComponent<ProjectileController>();

        // Definir a direção do projétil
        int projectileDirection = moveDirection;

        // Calcular o ângulo de lançamento do projétil (30 graus)
        float launchAngle = 50f;

        // Calcular a velocidade horizontal e vertical do projétil com base no ângulo e na velocidade
        float projectileSpeedX = projectileSpeed * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        float projectileSpeedY = projectileSpeed * Mathf.Sin(launchAngle * Mathf.Deg2Rad);

        // Definir a velocidade do projétil
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        projectileRb.velocity = new Vector2(projectileDirection * projectileSpeedX, projectileSpeedY);
    }

    private void OnDrawGizmosSelected()
    {
        // Desenhar o gizmo do alcance de perseguição
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}