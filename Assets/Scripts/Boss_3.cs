using System.Collections;
using UnityEngine;

public class Boss_3 : MonoBehaviour
{
    public Animator anim;

    private float tempoMinimoEntreAtaques=3.5f, tempoMaximoEntreAtaques = 10f;
    private bool isParado = false, podeAtacarNovamente=true;
    private int cont = 0;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(comecar());
    }

    void Update()
    {
        if (isParado && CordeiroScript.ativo)
        {
            if (podeAtacarNovamente)
            {
                StopAllCoroutines();
                StartCoroutine(atacar());
                Debug.Log("alaoalao");
            }
            else
                StopCoroutine(atacar());

            if (Input.GetKeyDown(KeyCode.C))
            {
                anim.SetBool("dano", true);
                cont++;
                isParado = false;
            }
        }
        if(cont == 4)
        {
            anim.SetBool("morte", true);
            isParado = false;
        }
    }

    private IEnumerator comecar()
    {
        CordeiroScript.ativo = false;  /*Destaivando os controles do personagem*/
        yield return new WaitForSeconds(7);
        CordeiroScript.ativo = true;  /*Reativando os controles do personagem*/
        isParado = true;
    }

    private IEnumerator atacar()
    {
        podeAtacarNovamente = false;
        yield return new WaitForSeconds(Random.Range(tempoMinimoEntreAtaques, tempoMaximoEntreAtaques));
        if (isParado)
        {
            anim.SetBool("ataque", true);
            isParado = false;
        }
        else
            podeAtacarNovamente = true;
    }

    private void terminarAtaque()
    {
        Debug.Log("Ataque acabou!");
        anim.SetBool("ataque", false);
        isParado = true;
        podeAtacarNovamente = true;

    }

    private void terminarDano()
    {
        Debug.Log("Dano acabou!");
        isParado = true;
        anim.SetBool("dano", false);
    }

    private void terminarMorte()
    {
        Debug.Log("Morte acabou!");
        Destroy(gameObject);
        GameObject.FindGameObjectWithTag("ProxFase").SetActive(true);     /*"Spawnando o objeto que fará o jogador zerar o jogo"*/
    }
}