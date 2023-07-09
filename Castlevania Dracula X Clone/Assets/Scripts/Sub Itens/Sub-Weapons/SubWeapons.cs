using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enumeração dos tipos de itens disponíveis
public enum ItemType
{
    None,       // Item vazio
    Faca,       // Item "Faca"
    Machado,    // Item "Machado"
    Cruz,       // Item "Cruz"
    AguaBenta   // Item "AguaBenta"
}

public class SubWeapons : MonoBehaviour
{
    // Referências aos objetos de imagem UI
    [SerializeField] private GameObject subItensImage;   // Objeto "SubItens" que contém todas as imagens dos itens
    [SerializeField] private GameObject facaImage;       // Imagem UI do item "Faca"
    [SerializeField] private GameObject machadoImage;    // Imagem UI do item "Machado"
    [SerializeField] private GameObject cruzImage;       // Imagem UI do item "Cruz"
    [SerializeField] private GameObject aguaBentaImage;  // Imagem UI do item "AguaBenta"

    // Item atualmente equipado
    public ItemType equippedItem = ItemType.None;

    private void Start()
    {
        // Desativar todas as imagens dos itens no início do jogo
        facaImage.SetActive(false);
        machadoImage.SetActive(false);
        cruzImage.SetActive(false);
        aguaBentaImage.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Faca"))
        {
            Destroy(collision.gameObject);
            machadoImage.SetActive(false);     // Desativar a imagem do machado na HUD
            cruzImage.SetActive(false);        // Desativar a imagem da cruz na HUD
            aguaBentaImage.SetActive(false);   // Desativar a imagem da agua benta na HUD
            facaImage.SetActive(true);         // Ativar a imagem da faca na HUD
            equippedItem = ItemType.Faca;      // Definir o item equipado como "Faca"
        }

        if (collision.gameObject.CompareTag("Machado"))
        {
            Destroy(collision.gameObject);
            facaImage.SetActive(false);        // Desativar a imagem da faca na HUD
            cruzImage.SetActive(false);        // Desativar a imagem da cruz na HUD
            aguaBentaImage.SetActive(false);   // Desativar a imagem da agua benta na HUD
            machadoImage.SetActive(true);      // Ativar a imagem do machado na HUD
            equippedItem = ItemType.Machado;   // Definir o item equipado como "Machado"
        }

        if (collision.gameObject.CompareTag("Cruz"))
        {
            Destroy(collision.gameObject);
            facaImage.SetActive(false);       // Desativar a imagem da faca na HUD
            machadoImage.SetActive(false);    // Desativar a imagem do machado na HUD
            aguaBentaImage.SetActive(false);  // Desativar a imagem da agua benta na HUD
            cruzImage.SetActive(true);        // Ativar a imagem da cruz na HUD
            equippedItem = ItemType.Cruz;     // Definir o item equipado como "Cruz"
        }

        if (collision.gameObject.CompareTag("Agua Benta"))
        {
            Destroy(collision.gameObject);
            facaImage.SetActive(false);        // Desativar a imagem da faca na HUD
            machadoImage.SetActive(false);     // Desativar a imagem do machado na HUD
            cruzImage.SetActive(false);        // Desativar a imagem da cruz na HUD
            aguaBentaImage.SetActive(true);    // Ativar a imagem da água benta na HUD
            equippedItem = ItemType.AguaBenta; // Definir o item equipado como "AguaBenta"
        }
    }
}