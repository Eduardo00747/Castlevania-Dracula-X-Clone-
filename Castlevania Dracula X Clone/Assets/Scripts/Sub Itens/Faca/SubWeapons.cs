using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubWeapons : MonoBehaviour
{
    public GameObject subItensImage; // Referência para o objeto de imagem UI "SubItens"

    public GameObject facaImage; // Referência para o objeto de imagem UI "Faca"
    public GameObject machadoImage; // Referência para o objeto de imagem UI "Machado"
    public GameObject cruzImage; // Referência para o objeto de imagem UI "Cruz"
    public GameObject aguaBentaImage; // Referência para o objeto de imagem UI "Agua Benta"

    private void Start()
    {
        // Desativa todas as imagens no início do jogo
        facaImage.SetActive(false);
        machadoImage.SetActive(false);
        cruzImage.SetActive(false);
        aguaBentaImage.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se colidiu com um objeto de tag "Faca"
        if (collision.gameObject.CompareTag("Faca"))
        {
            // Destroi o objeto "Faca"
            Destroy(collision.gameObject);

            // Ativa a imagem UI "Faca" na HUD
            facaImage.SetActive(true);

            // Desativa as outras imagens
            machadoImage.SetActive(false);
            cruzImage.SetActive(false);
            aguaBentaImage.SetActive(false);
        }

        // Verifica se colidiu com um objeto de tag "Machado"
        if (collision.gameObject.CompareTag("Machado"))
        {
            // Destroi o objeto "Machado"
            Destroy(collision.gameObject);

            // Ativa a imagem UI "Machado" na HUD
            machadoImage.SetActive(true);

            // Desativa as outras imagens
            facaImage.SetActive(false);
            cruzImage.SetActive(false);
            aguaBentaImage.SetActive(false);
        }

        // Verifica se colidiu com um objeto de tag "Cruz"
        if (collision.gameObject.CompareTag("Cruz"))
        {
            // Destroi o objeto "Cruz"
            Destroy(collision.gameObject);

            // Ativa a imagem UI "Cruz" na HUD
            cruzImage.SetActive(true);

            // Desativa as outras imagens
            facaImage.SetActive(false);
            machadoImage.SetActive(false);
            aguaBentaImage.SetActive(false);
        }

        // Verifica se colidiu com um objeto de tag "Agua Benta"
        if (collision.gameObject.CompareTag("Agua Benta"))
        {
            // Destroi o objeto "Agua Benta"
            Destroy(collision.gameObject);

            // Ativa a imagem UI "Agua Benta" na HUD
            aguaBentaImage.SetActive(true);

            // Desativa as outras imagens
            facaImage.SetActive(false);
            machadoImage.SetActive(false);
            cruzImage.SetActive(false);
        }
    }
}
