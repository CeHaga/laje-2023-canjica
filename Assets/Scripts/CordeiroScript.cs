using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class CordeiroScript : MonoBehaviour
{
    /*Definindo as constantes do movimento*/
    private const int VEL_ANDANDO = 60, VEL_CORRENDO = 150, FORCA_PULO = 50;
    private const float TEMPO_PULO = 0.25f;   /*Este será o tempo que o usuário terá que segurar o botão de pulo para que o personagem atinja sua altura máxima*/

    /*Variáveis do personagem*/
    private int velocidadeAtual;
    private float timerPulo;
    private bool isPulando, releasedJump=true;

    public BoxCollider2D hitbox1;
    public GameObject chao;
    private Rigidbody2D rig;
    private Animator animator;
    private SpriteRenderer sr;

    void Awake()
    {
        hitbox1 = GetComponent<BoxCollider2D>();
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        timerPulo = TEMPO_PULO;
    }

    void Update()
    {
        Move();
        Jump();
        Attack();
    }

    void Move()
    {
        /*Fazendo a funcionalidade de andar para a esquerda e direita*/
        float movimento = Input.GetAxis("Horizontal");    /*Se não pressionar nada, o valor é zero. Se for esquerda é -1 e direita é 1*/
        if (movimento < 0)
        {
            sr.flipX = true;    /*Rotacionando o personagem para a esquerda*/

            velocidadeAtual = VEL_ANDANDO;
            if (Input.GetKey(KeyCode.LeftShift))
                velocidadeAtual = VEL_CORRENDO;
            else
                velocidadeAtual = VEL_ANDANDO;

        }
        else if (movimento > 0)
        {
            sr.flipX = false;     /*Rotacionando o personagem para a direita*/

            velocidadeAtual = VEL_ANDANDO;
            if (Input.GetKey(KeyCode.LeftShift))
                velocidadeAtual = VEL_CORRENDO;
            else
                velocidadeAtual = VEL_ANDANDO;

        }
        else
            velocidadeAtual = 0;

        rig.velocity = new Vector2(movimento * velocidadeAtual, rig.velocity.y);     /*Usando o velocity para mover o personagem*/
        animator.SetFloat("Velocidade", velocidadeAtual);
    }

    void Jump()
    {
        /*Fazendo a funcionalidade do pulo*/
        if (Input.GetButton("Jump"))      /*Detectando se foi apertado o botão de pulo (por padrão é o space)*/
        {
            if (timerPulo > 0)                /*Fazendo o sistema de pular mais alto se segurar o botão de pulo por um tempo*/
                timerPulo -= Time.deltaTime;
            else
                rig.gravityScale = 40;

            if (!isPulando && releasedJump)            /*Se estiver no chão e o botão de pulo não estiver sendo segurado*/
            {
                rig.gravityScale = 0;
                rig.AddForce(new Vector2(0, FORCA_PULO), ForceMode2D.Impulse);
                animator.SetBool("NoAr", true);
                isPulando = true;
            }
            releasedJump = false;
        }
        if (Input.GetButtonUp("Jump"))     /*Quando soltar o botão de pulo*/
        {
            timerPulo = TEMPO_PULO;
            rig.gravityScale = 40;
            releasedJump = true;
        }
    }

    void Attack()
    {
        /*Fazendo a funcionalidade de hitbox e vida*/
        if (Input.GetKey(KeyCode.Z))
            hitbox1.enabled = true;
        else
            hitbox1.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.tag == "Ground")       /*Verificando se o personagem está em contato com algum gameObject com a tag "Ground"*/
        {
            isPulando = false;
            animator.SetBool("NoAr", false);
        }
    }

}