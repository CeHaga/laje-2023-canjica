using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadGame()
    {
        Transicao_Fases transicao = new Transicao_Fases();
        transicao.carregarProximaCena();    /*Carregando a primeira fase*/
    }

    public void QuitGame()
    {
        Application.Quit();    /*Saindo do jogo*/
    }
}
