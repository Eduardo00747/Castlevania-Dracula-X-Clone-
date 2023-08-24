using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Enumeração dos tipos de itens disponíveis
public enum ItemType
{
    None,
    Faca,
    Machado,
    Cruz,
    AguaBenta
}

public class SubWeapons : MonoBehaviour
{
    // Referências aos objetos de imagem UI
    [SerializeField] private GameObject subItensImage;   // Objeto "SubItens" que contém todas as imagens dos itens
    [SerializeField] private GameObject facaImage;       // Imagem UI do item "Faca"
    [SerializeField] private GameObject machadoImage;    // Imagem UI do item "Machado"
    [SerializeField] private GameObject cruzImage;       // Imagem UI do item "Cruz"
    [SerializeField] private GameObject aguaBentaImage;  // Imagem UI do item "AguaBenta"

    // Prefabs dos itens
    public GameObject facaPrefab;
    public GameObject machadoPrefab;
    public GameObject cruzPrefab;
    public GameObject aguaBentaPrefab;

    // Objeto para o spawn dos itens
    public GameObject dropItem;

    // Item atualmente equipado
    public ItemType equippedItem = ItemType.None;
    private ItemType previousEquippedItem = ItemType.None;
    private bool isFirstItemCollected = true;

    // Referência ao componente AudioSource no objeto do personagem
    private AudioSource audioSource;
    // Sound effect a ser reproduzido quando um dos itens for destruído
    public AudioClip soundWeapons;

    private void Start()
    {
        // Obter o componente AudioSource no objeto do personagem
        audioSource = GetComponent<AudioSource>();

        // Desativar todas as imagens dos itens no início do jogo
        facaImage.SetActive(false);
        machadoImage.SetActive(false);
        cruzImage.SetActive(false);
        aguaBentaImage.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar colisão com itens
        if (collision.gameObject.CompareTag("Faca"))
        {
            //Audio ao pegar corações 
            audioSource.PlayOneShot(soundWeapons);

            Destroy(collision.gameObject);

            if (!isFirstItemCollected)
            {
                SpawnPreviousItem();
            }

            // Ativar a imagem da faca na HUD e definir o item equipado como "Faca"
            machadoImage.SetActive(false);
            cruzImage.SetActive(false);
            aguaBentaImage.SetActive(false);
            facaImage.SetActive(true);
            equippedItem = ItemType.Faca;

            isFirstItemCollected = false;
        }

        if (collision.gameObject.CompareTag("Machado"))
        {
            //Audio ao pegar corações 
            audioSource.PlayOneShot(soundWeapons);

            Destroy(collision.gameObject);

            if (!isFirstItemCollected)
            {
                SpawnPreviousItem();
            }

            // Ativar a imagem do machado na HUD e definir o item equipado como "Machado"
            facaImage.SetActive(false);
            cruzImage.SetActive(false);
            aguaBentaImage.SetActive(false);
            machadoImage.SetActive(true);
            equippedItem = ItemType.Machado;

            isFirstItemCollected = false;
        }

        if (collision.gameObject.CompareTag("Cruz"))
        {
            //Audio ao pegar corações 
            audioSource.PlayOneShot(soundWeapons);

            Destroy(collision.gameObject);

            if (!isFirstItemCollected)
            {
                SpawnPreviousItem();
            }

            // Ativar a imagem da cruz na HUD e definir o item equipado como "Cruz"
            facaImage.SetActive(false);
            machadoImage.SetActive(false);
            aguaBentaImage.SetActive(false);
            cruzImage.SetActive(true);
            equippedItem = ItemType.Cruz;

            isFirstItemCollected = false;
        }

        if (collision.gameObject.CompareTag("Agua Benta"))
        {
            //Audio ao pegar corações 
            audioSource.PlayOneShot(soundWeapons);

            Destroy(collision.gameObject);

            if (!isFirstItemCollected)
            {
                SpawnPreviousItem();
            }

            // Ativar a imagem da água benta na HUD e definir o item equipado como "AguaBenta"
            facaImage.SetActive(false);
            machadoImage.SetActive(false);
            cruzImage.SetActive(false);
            aguaBentaImage.SetActive(true);
            equippedItem = ItemType.AguaBenta;

            isFirstItemCollected = false;
        }
    }

    private void SpawnPreviousItem()
    {
        // Spawn do item anteriormente equipado no objeto "Drop Item"
        switch (previousEquippedItem)
        {
            case ItemType.Faca:
                Instantiate(facaPrefab, dropItem.transform.position, Quaternion.identity);
                break;
            case ItemType.Machado:
                Instantiate(machadoPrefab, dropItem.transform.position, Quaternion.identity);
                break;
            case ItemType.Cruz:
                Instantiate(cruzPrefab, dropItem.transform.position, Quaternion.identity);
                break;
            case ItemType.AguaBenta:
                Instantiate(aguaBentaPrefab, dropItem.transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        // Atualizar o item equipado anteriormente
        previousEquippedItem = equippedItem;
    }
}