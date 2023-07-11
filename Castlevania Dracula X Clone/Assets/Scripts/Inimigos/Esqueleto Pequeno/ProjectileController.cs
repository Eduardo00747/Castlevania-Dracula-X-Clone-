using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            // Destruir o projetil quando colidir com o chão ou com o jogador
            Destroy(gameObject);
        }
    }
}