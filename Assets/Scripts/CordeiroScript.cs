using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordeiroScript : MonoBehaviour
{
    private int inputX, velocidade = 60, altura = 0;
    private float distancia;
    private bool pulo = false, olhandoEsquerda=false;
    public BoxCollider2D hitbox1;
    public Animator animator;
    private Vector2 vector2;
    public GameObject chao;

    void Awake()
    {
        hitbox1 = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        distancia = (int)transform.position.y - (int)chao.transform.position.y;
        movimento();
    }

    void movimento()
    {
        /*Fazendo a funcionalidade do pulo*/
        if (Input.GetKey("space") && (distancia < 50) && (!pulo))
        {   
            animator.SetBool("NoAr", true);
            altura += 6;
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
            altura -= 4;
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

        /*Fazendo a funcionalidade de hitbox e vida*/
        if (Input.GetKey(KeyCode.Z))
        {
            hitbox1.enabled = true;
        }
        else
        {
            hitbox1.enabled = false;
        }

        animator.SetFloat("Velocidade", velocidade);
        transform.position = new Vector2(transform.position.x + inputX * velocidade * Time.deltaTime, transform.position.y + altura * Time.deltaTime);
        vector2 = transform.position;
    }

}