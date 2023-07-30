using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    // Referência ao objeto do cursor
    public Transform cursorObject;

    // Posições do cursor
    private Vector3[] cursorPositions;

    // Índice da posição atual do cursor
    private int currentIndex = 0;

    private void Start()
    {
        // Defina as posições do cursor em um array
        cursorPositions = new Vector3[]
        {
            new Vector3(-2.7f, -1.3f, 0f),
            new Vector3(-2.7f, -2.4f, 0f),
            new Vector3(-2.7f, -3.6f, 0f)
        };

        // Posicione o cursor inicialmente na primeira posição
        cursorObject.position = cursorPositions[currentIndex];
    }

    private void Update()
    {
        // Verifique a entrada do jogador
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            MoveCursorDown();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            MoveCursorUp();
        }

        // Verifique se o jogador pressionou a tecla "K"
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Verifique a posição atual do cursor e carregue a cena correspondente
            if (currentIndex == 0)
            {
                SceneManager.LoadScene("Mapa Fase 1");
            }
            else if (currentIndex == 1)
            {
                SceneManager.LoadScene("PassWord");
            }
            else if (currentIndex == 2)
            {
                SceneManager.LoadScene("Options");
            }
        }
    }

    private void MoveCursorDown()
    {
        // Incrementar o índice para a próxima posição do cursor
        currentIndex++;

        // Certifique-se de que o índice esteja dentro dos limites do array
        if (currentIndex >= cursorPositions.Length)
        {
            currentIndex = 0;
        }

        // Atualize a posição do cursor
        cursorObject.position = cursorPositions[currentIndex];
    }

    private void MoveCursorUp()
    {
        // Decrementar o índice para a posição anterior do cursor
        currentIndex--;

        // Certifique-se de que o índice não seja menor que zero
        if (currentIndex < 0)
        {
            currentIndex = cursorPositions.Length - 1;
        }

        // Atualize a posição do cursor
        cursorObject.position = cursorPositions[currentIndex];
    }
}
