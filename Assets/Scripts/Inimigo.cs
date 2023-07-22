using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject barra_de_vida_verde, barra_de_vida_vermelha;
    private double vida = 10, danoRecebido = 5;
    public Animator animator;

    void Awake()
    {
        
    }

    void Update()
    {

        if(vida <= 0) /*Reduz a escala do GameObject á 0 quando a vida chega a zero*/
        {
            if(transform.localScale.x >= 0)
            {   
                transform.localScale = new Vector3(transform.localScale.x - (float)0.28, transform.localScale.y , transform.localScale.z);
            }
            if(transform.localScale.y >= 0)
            {   
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - (float)0.28, transform.localScale.z);
            }
            if(transform.localScale.z >= 0)
            {   
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - (float)0.28);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) /*Tratamento de eventos do tipo Trigger com colisões*/
    {
        Debug.Log("Acertou!");
        recebeDano();
    }

    void recebeDano() /*Função chamada ao receber dano*/
    {
        vida -= danoRecebido;
        if(vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            animator.SetBool("Vida", false);
        }

        barra_de_vida_verde.transform.localScale = new Vector3((float)vida / 10, barra_de_vida_verde.transform.localScale.y, barra_de_vida_verde.transform.localScale.z); /*Diminui a barra de vida verde*/
    }
}
