using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transicao_Fases : MonoBehaviour
{
    public Animator anim;
    private float tempoTransicao = 0.2f;
    public static bool transicao = false;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {   /*Se não for o menu*/
            anim.SetBool("aparecerTela", true);
        }
    }
    private void Update()
    {
        if (transicao)
        {
            carregarProximaCena();
            transicao = false;
        }
    }

    private void terminarAnimacaoFadeOut()
    {
        anim.SetBool("aparecerTela", false);
    }

    public void carregarProximaCena()
    {
        StartCoroutine(carregarLevel(SceneManager.GetActiveScene().buildIndex + 1));    /*Chamando a co-rotina que carrega a próxima cena*/
    }

    private IEnumerator carregarLevel(int index)
    {
        anim.SetBool("escurecerTela", true);     /*Ativando a animação de transição entre telas*/
        yield return new WaitForSeconds(tempoTransicao);
        SceneManager.LoadScene(index);
    }
}
