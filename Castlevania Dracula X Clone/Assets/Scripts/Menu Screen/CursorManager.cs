using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{
    // Refer�ncia ao objeto do cursor
    public Transform cursorObject;

    // Posi��es do cursor
    private Vector3[] cursorPositions;

    // �ndice da posi��o atual do cursor
    private int currentIndex = 0;

    private void Start()
    {
        // Defina as posi��es do cursor em um array
        cursorPositions = new Vector3[]
        {
            new Vector3(-2.7f, -1.3f, 0f),
            new Vector3(-2.7f, -2.4f, 0f),
            new Vector3(-2.7f, -3.6f, 0f)
        };

        // Posicione o cursor inicialmente na primeira posi��o
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
            // Verifique a posi��o atual do cursor e carregue a cena correspondente
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
        // Incrementar o �ndice para a pr�xima posi��o do cursor
        currentIndex++;

        // Certifique-se de que o �ndice esteja dentro dos limites do array
        if (currentIndex >= cursorPositions.Length)
        {
            currentIndex = 0;
        }

        // Atualize a posi��o do cursor
        cursorObject.position = cursorPositions[currentIndex];
    }

    private void MoveCursorUp()
    {
        // Decrementar o �ndice para a posi��o anterior do cursor
        currentIndex--;

        // Certifique-se de que o �ndice n�o seja menor que zero
        if (currentIndex < 0)
        {
            currentIndex = cursorPositions.Length - 1;
        }

        // Atualize a posi��o do cursor
        cursorObject.position = cursorPositions[currentIndex];
    }
}
