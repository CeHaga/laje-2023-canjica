using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.SceneManagement;

public class CordeiroScript : MonoBehaviour
{
    /*Definindo as constantes do movimento*/
    private const float VEL_ANDANDO = 1.5f, VEL_CORRENDO = 3, FORCA_PULO = 1;
    private const float TEMPO_PULO = 0.25f;   /*Este será o tempo que o usuário terá que segurar o botão de pulo para que o personagem atinja sua altura máxima*/

    /*Variáveis do personagem*/
    private float velocidadeAtual;
    private float timerPulo;
    private bool isPulando, releasedJump=true;

    public BoxCollider2D hitbox1;
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
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Cam.numInimigosDerrotados++;
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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))      /*Detectando se foi apertado o botão de pulo (no nosso caso será o W ou seta para cima)*/
        {
            if (timerPulo > 0)                /*Fazendo o sistema de pular mais alto se segurar o botão de pulo por um tempo*/
                timerPulo -= Time.deltaTime;
            else
                rig.gravityScale = 1;

            if (!isPulando && releasedJump)            /*Se estiver no chão e o botão de pulo não estiver sendo segurado*/
            {
                rig.gravityScale = 0;
                rig.AddForce(new Vector2(0, FORCA_PULO), ForceMode2D.Impulse);    /*Adicionando força ao rigidbody para fazer o personagem pular*/
                animator.SetBool("NoAr", true);
                isPulando = true;
            }
            releasedJump = false;
        }
        if ((Input.GetKeyUp(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)) || (Input.GetKeyUp(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W)))     /*Quando soltar o botão de pulo*/
        {
            timerPulo = TEMPO_PULO;
            rig.gravityScale = 1;
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

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        if (colisao.gameObject.tag == "ProxFase")      /*Verificando se o personagem encostou no trigger que o leva para a próxima fase*/
        {
            Debug.Log("Passou de fase");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}