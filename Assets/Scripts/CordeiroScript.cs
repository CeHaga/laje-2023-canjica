using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordeiroScript : MonoBehaviour
{
    public int dano = 5;
    private float vida = 1000, danoPercentual = 0, timerPulo, velocidadeAtual;
    private bool olhandoEsquerda = false, ataqueEmAndamento = false, desvioEmAndamento = false, isPulando, releasedJump = true;
    public BoxCollider2D hitbox, hurtbox;

    private Animator animator;
    public Inimigo inimigo;
    private Rigidbody2D rig;
    private SpriteRenderer sr;

    /*Definindo as constantes do movimento*/
    private const float VEL_ANDANDO = 1.5f, VEL_CORRENDO = 3, FORCA_PULO = 1;
    private const float TEMPO_PULO = 0.25f; /*Este será o tempo que o usuário terá que segurar o botão de pulo para que o personagem atinja sua altura máxima*/

    public static bool ativo = true; /*Esta variável será usada para desativar os controles do personagem em certos pontos do jogo*/


    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        timerPulo = TEMPO_PULO;

    }

    void Update()
    {
        if (ativo)
        {
            Move();
            Jump();

            if (Input.GetKey(KeyCode.Z) && !ataqueEmAndamento)
                StartCoroutine(Attack());

            if (Input.GetKey(KeyCode.X) && !desvioEmAndamento)
                StartCoroutine(desvio());

            if (Input.GetKeyDown(KeyCode.LeftControl))
                Cam.numInimigosDerrotados++;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "HitboxRange")
        {
            inimigo.hitboxRange = true;
            inimigo.visão = false;
            Debug.Log("Tomei dano ai");
        }

        if (other.tag == "VisaoInimigo")
        {
            inimigo.visão = true;
        }

        if (other.gameObject.tag == "ProxFase") /*Verificando se o personagem encostou no trigger que o leva para a próxima fase*/
        {
            Debug.Log("Passou de fase");
            Transicao_Fases.transicao = true; /*Chamando a função de carregar a próxima cena*/
        }

        if (other.tag == "AtaqueInimigo")
        {
            StartCoroutine(recebeDano());
        }

        if (other.tag == "Inimigo")
        {
            inimigo.ataquePlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "VisaoInimigo")
            inimigo.visão = false;

        if (other.tag == "HitboxRange")
        {
            inimigo.hitboxRange = false;
            inimigo.visão = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.tag == "Ground") /*Verificando se o personagem está em contato com algum gameObject com a tag "Ground"*/
        {
            isPulando = false;
            animator.SetBool("NoAr", false);
        }
    }


    void Move()
    {
        /*Fazendo a funcionalidade de andar para a esquerda e direita*/
        float movimento = Input.GetAxis("Horizontal");    /*Se não pressionar nada, o valor é zero. Se for esquerda é -1 e direita é 1*/

        if (movimento < 0)
        {
            if (!olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);    /*Rotacionando o personagem para a esquerda*/
                olhandoEsquerda = !olhandoEsquerda;
            }

            velocidadeAtual = VEL_ANDANDO;
            if (Input.GetKey(KeyCode.LeftShift))
                velocidadeAtual = VEL_CORRENDO;
            else
                velocidadeAtual = VEL_ANDANDO;

        }
        else if (movimento > 0)
        {
            if (olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);    /*Rotacionando o personagem para a direita*/
                olhandoEsquerda = !olhandoEsquerda;
            }

            velocidadeAtual = VEL_ANDANDO;
            if (Input.GetKey(KeyCode.LeftShift))
                velocidadeAtual = VEL_CORRENDO;
            else
                velocidadeAtual = VEL_ANDANDO;

        }
        else
        {
            velocidadeAtual = 0;
        }
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

    IEnumerator Attack()
    {
        animator.SetBool("Ataque", true);

        yield return new WaitForSeconds(0.2f); /* Aguardar até a animação do hit efetivamente aconteça.*/

        if (animator.GetBool("Ataque"))
        {
            ataqueEmAndamento = true;
            hitbox.enabled = true;
        }

        yield return new WaitForSeconds(0.18f); /* Aguardar até a animação do hit efetivamente aconteça.*/

        hitbox.enabled = false;

        animator.SetBool("Ataque", false);
        ataqueEmAndamento = false;
    }

    IEnumerator recebeDano() /*Função chamada ao receber dano*/
    {
        if (danoPercentual == 0)
            danoPercentual = (inimigo.dano / vida);

        vida -= inimigo.dano;

        if (vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            animator.SetBool("Vida", false);
        }

        //Mudança de cor ao tomar dano

        sr.color = new Color(0.85f, 0.21f, 0.21f, 1);

        yield return new WaitForSeconds(0.2f);

        sr.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.2f);

        sr.color = new Color(0.85f, 0.21f, 0.21f, 1);

        yield return new WaitForSeconds(0.2f);

        sr.color = new Color(1, 1, 1, 1);

    }

    IEnumerator desvio()
    {
        animator.SetBool("Desvio", true);

        desvioEmAndamento = true;
        hurtbox.enabled = false;

        yield return new WaitForSeconds(0.4f);

        hurtbox.enabled = true;
        desvioEmAndamento = false;
        animator.SetBool("Desvio", false);

    }

}