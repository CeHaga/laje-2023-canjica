using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject telaPause, telaGameOver;   /*Estes serão os objetos referentes às telas de Pause e Game Over*/
    public GameObject paredeInvisivel1, paredeInvisivel2;

    private bool isPausado = false, isMorto = false, acabouMostrarTelaGameOver = false;


    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Fase"))   /*Se for uma fase normal*/
        {
            if (isMorto && !acabouMostrarTelaGameOver)
                acabouMostrarTelaGameOver = mostrarTelaGameover();

            if (Input.GetKeyDown(KeyCode.Escape) && !isMorto)
                isPausado = !isPausado;
            telaPause.SetActive(isPausado);    /*Ativando ou desativando a tela de Pause*/

            if (Input.GetKeyDown(KeyCode.S) && !isMorto)
            {
                isMorto = true;
                telaGameOver.SetActive(true);
            }

            if (isPausado)
                Time.timeScale = 0;    /*Parando todos os processos do jogo*/
            else
                Time.timeScale = 1;    /*Reiniciando todos os processos do jogo*/
        }


        if (Input.anyKeyDown)
        {
            if (SceneManager.GetActiveScene().name.Contains("Texto"))
                Transicao_Fases.transicao = true;    /*Carregando a próxima fase*/
            else if (SceneManager.GetActiveScene().name.Contains("Final"))
                QuitGame();    /*Saindo do jogo depois da tela final*/
        }
    }

    public bool mostrarTelaGameover()     /*Esta função faz com que a tela de Game Over apareça com um efeito de fade in*/
    {
        Transform[] objetosFilhos = telaGameOver.GetComponentsInChildren<Transform>();
        int cont = 0;
        for (int i = 0; i < objetosFilhos.Length; i++)
        {
            if (objetosFilhos[i].GetComponent<Image>() != null)     /*Se for uma imagem*/
            {
                Color novaCorr = objetosFilhos[i].GetComponent<Image>().color;
                novaCorr.a = 110;
                objetosFilhos[i].GetComponent<Image>().color = Color.Lerp(objetosFilhos[i].GetComponent<Image>().color, novaCorr, Time.deltaTime * 0.003f);
                if (objetosFilhos[i].GetComponent<Image>().color.a >= 1)
                    cont++;
            }
            else      /*Se for um texto*/
            {
                Color novaCorr = objetosFilhos[i].GetComponent<TextMeshProUGUI>().color;
                novaCorr.a = 110;
                objetosFilhos[i].GetComponent<TextMeshProUGUI>().color = Color.Lerp(objetosFilhos[i].GetComponent<TextMeshProUGUI>().color, novaCorr, Time.deltaTime * 0.003f);
                if (objetosFilhos[i].GetComponent<TextMeshProUGUI>().color.a >= 1)
                    cont++;
            }
        }
        if (cont == objetosFilhos.Length)
            return true;

        return false;
    }

    /*Funcionalidades de botões*/
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");    /*Carregando o menu*/
        Time.timeScale = 1;
        isMorto = false;
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      /*Carregando novamente a fase na qual o jogador se encontra*/
        Time.timeScale = 1;
        isMorto = false;
    }

    public void LoadGame()
    {
        Transicao_Fases.transicao = true;    /*Carregando a próxima fase*/
    }

    public void QuitGame()
    {
        Application.Quit();    /*Saindo do jogo*/
    }

    public void Configuracoes()
    {
        SceneManager.LoadScene("Configuracoes");
    }
}