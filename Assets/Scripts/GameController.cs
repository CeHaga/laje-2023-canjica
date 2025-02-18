using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject telaPause, telaGameOver;   /*Estes ser�o os objetos referentes �s telas de Pause e Game Over*/
    public GameObject paredeInvisivel1, paredeInvisivel2;

    private bool isPausado = false, isMorto = false, acabouMostrarTelaGameOver = false;

    public AudioSource somTocha, musicaGameOver, somBotao, somMenu;

    public static bool morreu = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Contains("Corredor"))
            somTocha.Play();
        else if (SceneManager.GetActiveScene().name.Contains("Menu"))
            somMenu.Play();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name.Contains("Fase"))   /*Se for uma fase normal*/
        {
            if (isMorto && !acabouMostrarTelaGameOver)
                acabouMostrarTelaGameOver = mostrarTelaGameover();

            if (Input.GetKeyDown(KeyCode.Escape) && !isMorto)
                isPausado = !isPausado;
            telaPause.SetActive(isPausado);    /*Ativando ou desativando a tela de Pause*/

            //if (Input.GetKeyDown(KeyCode.S) && !isMorto)
            if ((morreu && !isMorto) || Input.GetKey(KeyCode.Alpha9))
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
                Transicao_Fases.transicao = true;    /*Carregando a pr�xima fase*/
            else if (SceneManager.GetActiveScene().name.Contains("Fim"))
                QuitGame();    /*Saindo do jogo depois da tela final*/
        }
    }

    public bool mostrarTelaGameover()     /*Esta fun��o faz com que a tela de Game Over apare�a com um efeito de fade in*/
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
        {
            musicaGameOver.Play();
            return true;
        }

        return false;
    }

    /*Funcionalidades de bot�es*/
    public void ReturnToMenu()
    {
        tocarSomBotao();
        StartCoroutine(ReturnToMenuAtrasado());
    }
    IEnumerator ReturnToMenuAtrasado()
    {
        yield return new WaitForSeconds(0.5f);
        if (!SceneManager.GetActiveScene().name.Contains("Boss"))
            AudioController.GetInstance().StopAudio();
        SceneManager.LoadScene("Menu");    /*Carregando o menu*/
        if (musicaGameOver != null)
            musicaGameOver.Stop();
        Time.timeScale = 1;
        isMorto = false;
        morreu = false;
    }
    public void RestartLevel()
    {
        tocarSomBotao();
        StartCoroutine(RestartLevelAtrasado());
    }
    IEnumerator RestartLevelAtrasado()
    {
        yield return new WaitForSeconds(0.5f);
        CordeiroScript.ativo = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);      /*Carregando novamente a fase na qual o jogador se encontra*/
        musicaGameOver.Stop();
        Time.timeScale = 1;
        isMorto = false;
        morreu = false;
    }

    public void LoadGame()
    {
        tocarSomBotao();
        StartCoroutine(LoadGameAtrasado());
    }
    IEnumerator LoadGameAtrasado()
    {
        yield return new WaitForSeconds(0.5f);
        Transicao_Fases.transicao = true;    /*Carregando a pr�xima fase*/
    }

    public void QuitGame()
    {
        tocarSomBotao();
        StartCoroutine(QuitGameAtrasado());
    }
    IEnumerator QuitGameAtrasado()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();    /*Saindo do jogo*/
    }

    public void Configuracoes()
    {
        tocarSomBotao();
        StartCoroutine(ConfiguracoesAtrasado());
    }
    IEnumerator ConfiguracoesAtrasado()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Configuracoes");
    }

    public void tocarSomBotao()
    {
        somBotao.Play();
    }
}