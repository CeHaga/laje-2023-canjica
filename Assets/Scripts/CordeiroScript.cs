using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordeiroScript : MonoBehaviour
{
    public int inputX, dano = 5, velocidade = 60, velocidadePulo = 5;
    private float vida = 80, danoPercentual = 0;
    private float distancia, altura = 0;
    private bool pulo = false, olhandoEsquerda=false, ataqueEmAndamento = false;
    public BoxCollider2D hitbox1, hitbox2;
    public Animator animator;
    public Enemy inimigo;
    private Vector2 vector2;
    public GameObject chao;

    void Awake()
    {
        hitbox1 = gameObject.GetComponent<BoxCollider2D>();
        hitbox2 = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {   

        distancia = (int)transform.position.y - (int)chao.transform.position.y;

        movimento();
        if(Input.GetKey("z") && !ataqueEmAndamento)
        {
            StartCoroutine(ataque());
        }
    }

    void movimento()
    {
        /*Fazendo a funcionalidade do pulo*/
        if (Input.GetKey("space") && (distancia < 50) && (!pulo))
        {   
            animator.SetBool("NoAr", true);
            altura += 200.0f * velocidadePulo * Time.deltaTime;
        }
        else if ((distancia <= 25))
        {
            animator.SetBool("NoAr", false);
            altura = 0;
            pulo = false;
        }
        else if ((distancia >= 50))
        {
            animator.SetBool("NoAr", true);
            altura -= 1500.0f * Time.deltaTime;
            pulo = true;
        }

        /*Fazendo a funcionalidade de andar para a esquerda e direita*/
        if (Input.GetKey("left"))
        {
            if (!olhandoEsquerda)    /*Rotacionando o personagem*/
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = true;
            }
            inputX = -1;
            velocidade = 60;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velocidade = 150;
            }
            else
            {
                velocidade = 60;
            }

        }
        else if (Input.GetKey("right"))
        {
            if (olhandoEsquerda)       /*Rotacionando o personagem*/
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = false;
            }
            inputX = 1;
            velocidade = 60;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                velocidade = 150;
            }
            else
            {
                velocidade = 60;
            }

        }
        else
        {
            velocidade = 0;
            inputX = 0;
        }

        animator.SetFloat("Velocidade", velocidade);
        transform.position = new Vector2(transform.position.x + inputX * velocidade * Time.deltaTime, transform.position.y + altura * Time.deltaTime);
        vector2 = transform.position;
    
    }
    IEnumerator ataque()
    {   
            animator.SetBool("Ataque", true);

            yield return new WaitForSeconds(0.2f); /* Aguardar até a animação do hit efetivamente aconteça.*/

            if(animator.GetBool("Ataque"))
            {
                ataqueEmAndamento = true;
                hitbox1.enabled = true;
            }

            yield return new WaitForSeconds(0.2f); /* Aguardar até a animação do hit efetivamente aconteça.*/

            hitbox1.enabled = false;

            animator.SetBool("Ataque", false);
            ataqueEmAndamento = false;
    }

}