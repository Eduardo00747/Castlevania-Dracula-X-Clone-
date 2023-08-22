using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogoAgua : MonoBehaviour
{
    private Transform ritcherTransform;
    private SpriteRenderer ritcherSpriteRenderer;
    private bool isMoving = false;
    private Vector3 moveDirection;

    private void Start()
    {
        ritcherTransform = GameObject.Find("Ritcher").transform; // Substitua "Ritcher" pelo nome do objeto do personagem
        ritcherSpriteRenderer = ritcherTransform.GetComponent<SpriteRenderer>();

        // Destroi o objeto após 2 segundos
        Invoke("DestruirObjeto", 0.5f);
    }

    private void Update()
    {
        if (!isMoving)
        {
            // Verificar o flip em X do personagem (Ritcher)
            bool isFlipped = ritcherSpriteRenderer.flipX;

            // Armazenar a direção de movimento inicial
            moveDirection = isFlipped ? Vector3.left : Vector3.right;

            isMoving = true; // Iniciar o movimento
        }

        // Movimentar o objeto na direção armazenada
        transform.Translate(moveDirection * Time.deltaTime);
    }

    void DestruirObjeto()
    {
        // Destroi o objeto ao qual o script está adicionado
        Destroy(gameObject);
    }
}