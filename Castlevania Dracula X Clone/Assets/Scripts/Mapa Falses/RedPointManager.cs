using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RedPointManager : MonoBehaviour
{
    public GameObject ponto1;
    public GameObject ponto2;
    public GameObject ponto3;
    public GameObject caveira;

    // Start is called before the first frame update
    void Start()
    {
        // Inicia a corrotina para ativar os objetos em sequência
        StartCoroutine(ActivateObjectsWithDelay());
    }

    // Corrotina para ativar os objetos em sequência com um atraso
    private IEnumerator ActivateObjectsWithDelay()
    {
        // Aguarda 0.5 segundos antes de ativar o objeto "Ponto 1"
        yield return new WaitForSeconds(0.5f);
        ponto1.SetActive(true);

        // Aguarda mais 0.5 segundos antes de ativar o objeto "Ponto 2"
        yield return new WaitForSeconds(0.5f);
        ponto2.SetActive(true);

        // Aguarda mais 0.5 segundos antes de ativar o objeto "Ponto 3"
        yield return new WaitForSeconds(0.5f);
        ponto3.SetActive(true);

        // Aguarda mais 0.5 segundos antes de ativar o objeto "Caveira"
        yield return new WaitForSeconds(0.5f);
        caveira.SetActive(true);

        // Aguarda mais 1 segundo antes de carregar a cena "Fase 1"
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Fase 1");
    }
}
