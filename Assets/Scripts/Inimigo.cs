using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject vidaVerde, vidaVermelha;
    public float vida = 100, danoPercentual = 0;
    private float corPercentual = 1;
    public Animator animator;
    public SpriteRenderer cor;
    public CordeiroScript cordeiro;
    private Vector2 targetPosition;

    void Awake()
    {
    }

    void Update()
    {   

        if(vida <= 0) /*Reduz a escala do GameObject á 0 quando a vida chega a zero*/
        {
            if(transform.localScale.x >= 0)
            {   
                transform.localScale = new Vector3(transform.localScale.x - 0.28f, transform.localScale.y , transform.localScale.z);
            }
            if(transform.localScale.y >= 0)
            {   
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.28f, transform.localScale.z);
            }
            if(transform.localScale.z >= 0)
            {   
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - 0.28f);
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

        if(danoPercentual == 0)
        {
            danoPercentual = (cordeiro.dano/vida);
        }


        corPercentual -= danoPercentual;

        cor.color = new Color (0, 1, 0.12f, corPercentual);
        vida -= cordeiro.dano;

        Debug.Log(danoPercentual);
        Debug.Log(corPercentual);


        if(vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            animator.SetBool("Vida", false);
        }
    }
}
