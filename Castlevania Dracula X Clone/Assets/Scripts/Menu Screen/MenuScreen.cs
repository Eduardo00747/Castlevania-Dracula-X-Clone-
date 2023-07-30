using System.Collections;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    public GameObject menuObject;
    public GameObject fundoObject;
    public GameObject logoObject;
    public GameObject optionsObject;

    // Start is called before the first frame update
    void Start()
    {
        // Iniciar a corrotina para controlar a sequ�ncia de ativa��o e desativa��o
        StartCoroutine(ActivateAndDeactivateObjects());
    }

    // Corrotina para controlar a sequ�ncia de ativa��o e desativa��o
    private IEnumerator ActivateAndDeactivateObjects()
    {
        // Aguardar o t�rmino da anima��o do objeto "Menu"
        Animator menuAnimator = menuObject.GetComponent<Animator>();
        yield return new WaitForSeconds(menuAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Desativar o objeto "Menu"
        menuObject.SetActive(false);

        // Aguardar um pequeno atraso antes de ativar o objeto "Fundo"
        yield return new WaitForSeconds(0.1f);
        fundoObject.SetActive(true);

        // Aguardar mais um pequeno atraso antes de ativar o objeto "Logo"
        yield return new WaitForSeconds(0.5f);
        logoObject.SetActive(true);

        // Aguardar um pequeno atraso antes de ativar o objeto "Options"
        yield return new WaitForSeconds(0.5f);
        optionsObject.SetActive(true);
    }
}
