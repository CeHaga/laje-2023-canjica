using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject telaPause, telaGameOver;   /*Estes serão os objetos referentes às telas de Pause e Game Over*/
    private bool isPausado = false, isMorto = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isMorto)
            isPausado = !isPausado;
        if (Input.GetKeyDown(KeyCode.Escape))
            isPausado = false;
        telaPause.SetActive(isPausado);    /*Ativando ou desativando a tela de Pause*/
        if (Input.GetKeyDown(KeyCode.S))
        {
            telaGameOver.SetActive(true);
            isMorto = true;
        }

        if (isPausado || isMorto)
            Time.timeScale = 0;    /*Parando todos os preocessos do jogo*/
        else
            Time.timeScale = 1;    /*Reiniciando todos os preocessos do jogo*/
    }


    /*Funcionalidades de botões*/
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);    /*Carregando o menu*/
        Time.timeScale = 1;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      /*Carregando novamente a fase na qual o jogador se encontra*/
        Time.timeScale = 1;
    }


    
}
