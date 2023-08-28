using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBatMorcego : MonoBehaviour
{
    public int maxHealth = 100; // Vida m�xima do inimigo
    private int currentHealth; // Vida atual do inimigo

    private void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida atual com o valor m�ximo
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount; // Reduz a vida atual pelo valor do dano

        // Verifica se a vida chegou a zero ou menos
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Aqui voc� pode adicionar a l�gica para o que acontece quando o inimigo morre,
        // como tocar uma anima��o de morte, soltar itens, etc.
        // Por enquanto, vamos apenas destruir o objeto.

        // Destruir o objeto do inimigo
        Destroy(gameObject);

        Debug.Log("O inimigo morreu!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se houve colis�o com um ataque
        if (collision.CompareTag("Ataque") || collision.gameObject.CompareTag("Machado") || collision.gameObject.CompareTag("Cruz") || collision.gameObject.CompareTag("Faca"))
        {
            // Reduz a vida do inimigo
            TakeDamage(10);
        }
    }
}