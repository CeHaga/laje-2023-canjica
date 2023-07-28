using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1);    /*Carregando a primeira fase*/
    }

    public void QuitGame()
    {
        Application.Quit();    /*Saindo do jogo*/
    }
}
