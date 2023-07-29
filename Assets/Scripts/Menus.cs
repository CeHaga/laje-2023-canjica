using UnityEngine;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour
{

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (SceneManager.GetActiveScene().name.Contains("Texto"))
                Transicao_Fases.transicao = true;    /*Carregando a próxima fase*/
        }
    }
    public void LoadGame()
    {
        Transicao_Fases.transicao = true;    /*Carregando a próxima fase*/
    }

    public void QuitGame()
    {
        Application.Quit();    /*Saindo do jogo*/
    }
}