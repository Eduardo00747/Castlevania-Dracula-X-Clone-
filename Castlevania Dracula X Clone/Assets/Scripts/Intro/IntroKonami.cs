using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroKonami : MonoBehaviour
{
    // Referências para os objetos que serão ativados e desativados
    public GameObject dataObject;
    public GameObject konamiIntroObject;

    // Tempo de atraso para ativar o objeto "Konami Intro" após ativar o objeto "Data"
    public float delay = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        // Iniciar a corrotina para controlar a sequência de ativação e desativação
        StartCoroutine(ActivateAndDeactivateObjects());
    }

    // Corrotina para controlar a sequência de ativação e desativação
    private IEnumerator ActivateAndDeactivateObjects()
    {
        // Ativar o objeto "Data"
        dataObject.SetActive(true);

        // Aguardar o tempo de atraso
        yield return new WaitForSeconds(delay);

        // Desativar o objeto "Data"
        dataObject.SetActive(false);

        // Ativar o objeto "Konami Intro"
        konamiIntroObject.SetActive(true);

        // Aguardar um pequeno atraso antes de mudar para a cena "Fase 1"
        yield return new WaitForSeconds(5.0f);

        // Mudar para a cena "Fase 1"
        SceneManager.LoadScene("Menu");
    }
}
