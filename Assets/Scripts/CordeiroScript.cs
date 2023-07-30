using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordeiroScript : MonoBehaviour
{
    public int dano = 5;
    private float vida = 100, danoPercentual = 0, timerPulo, velocidadeAtual;
    private bool olhandoEsquerda = false, ataqueEmAndamento = false, desvioEmAndamento = false, isPulando, releasedJump = true;
    public BoxCollider2D hitbox, rigidbox;

    private Animator animator;
    public Inimigo inimigo;
    private Rigidbody2D rig;
    private SpriteRenderer sr;

    /*Definindo as constantes do movimento*/
    private const float VEL_ANDANDO = 1.5f, VEL_CORRENDO = 3, FORCA_PULO = 1;
    private const float TEMPO_PULO = 0.25f; /*Este ser� o tempo que o usu�rio ter� que segurar o bot�o de pulo para que o personagem atinja sua altura m�xima*/

    public static bool ativo = true; /*Esta vari�vel ser� usada para desativar os controles do personagem em certos pontos do jogo*/

    public AudioSource somAtaque, somPasso, somDano, somChifreColetavel;


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

            if (Input.GetKey(KeyCode.Z) && !ataqueEmAndamento && !desvioEmAndamento)
                StartCoroutine(Attack());

            if (Input.GetKey(KeyCode.X) && !desvioEmAndamento)
                StartCoroutine(Desvio());
        }
        else
        {
            rigidbox.size = new Vector2(rigidbox.size.x, rigidbox.size.y - 0.6f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "HitboxRange")
        {
            inimigo.hitboxRange = true;
            inimigo.visao = false;
            Debug.Log("Tomei dano ai");
        }

        if (other.tag == "VisaoInimigo" && !ataqueEmAndamento)
        {
            inimigo.visao = true;
        }

        if (other.gameObject.tag == "ProxFase") /*Verificando se o personagem encostou no trigger que o leva para a pr�xima fase*/
        {
            Debug.Log("Passou de fase");
            Transicao_Fases.transicao = true; /*Chamando a fun��o de carregar a pr�xima cena*/
        }

        if (other.tag == "AtaqueInimigo" && !desvioEmAndamento)
        {
            StartCoroutine(recebeDano());
        }

        if (other.tag == "Inimigo")
        {
            inimigo.ataquePlayer = true;
        }

        if (other.tag == "Chifre")
        {
            tocarSomChifreColetavel();
            Destroy(other);
        }
    }

     void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Inimigo")
            inimigo.ataquePlayer = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "VisaoInimigo" && !ataqueEmAndamento)
            inimigo.visao = false;

        if (other.tag == "HitboxRange")
        {
            inimigo.hitboxRange = false;
            inimigo.visao = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.tag == "Ground") /*Verificando se o personagem est� em contato com algum gameObject com a tag "Ground"*/
        {
            isPulando = false;
            animator.SetBool("NoAr", false);
        }
        
    }


    void Move()
    {
        /*Fazendo a funcionalidade de andar para a esquerda e direita*/
        float movimento = Input.GetAxis("Horizontal");    /*Se n�o pressionar nada, o valor � zero. Se for esquerda � -1 e direita � 1*/

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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))      /*Detectando se foi apertado o bot�o de pulo (no nosso caso ser� o W ou seta para cima)*/
        {
            if (timerPulo > 0)                /*Fazendo o sistema de pular mais alto se segurar o bot�o de pulo por um tempo*/
                timerPulo -= Time.deltaTime;
            else
                rig.gravityScale = 1;

            if (!isPulando && releasedJump)            /*Se estiver no ch�o e o bot�o de pulo n�o estiver sendo segurado*/
            {
                rig.gravityScale = 0;
                rig.AddForce(new Vector2(0, FORCA_PULO), ForceMode2D.Impulse);    /*Adicionando for�a ao rigidbody para fazer o personagem pular*/
                animator.SetBool("NoAr", true);
                isPulando = true;
            }
            releasedJump = false;
        }
        if ((Input.GetKeyUp(KeyCode.W) && !Input.GetKey(KeyCode.UpArrow)) || (Input.GetKeyUp(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W)))     /*Quando soltar o bot�o de pulo*/
        {
            timerPulo = TEMPO_PULO;
            rig.gravityScale = 1;
            releasedJump = true;
        }
    }

    IEnumerator Attack()
    {
        animator.SetBool("Ataque", true);

        yield return new WaitForSeconds(0.2f); /* Aguardar at� a anima��o do hit efetivamente aconte�a.*/

        if (animator.GetBool("Ataque"))
        {
            ataqueEmAndamento = true;
            hitbox.enabled = true;
        }

        yield return new WaitForSeconds(0.18f); /* Aguardar at� a anima��o do hit efetivamente aconte�a.*/

        hitbox.enabled = false;

        animator.SetBool("Ataque", false);
        ataqueEmAndamento = false;
    }

    IEnumerator recebeDano() /*Fun��o chamada ao receber dano*/
    {
        if (danoPercentual == 0)
            danoPercentual = (inimigo.dano / vida);

        vida -= inimigo.dano;

        if (vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            ativo = false;
            animator.SetBool("Vida", false);
            tocarSomDano();

            yield return new WaitForSeconds(0.2f);
        }

        //Mudan�a de cor ao tomar dano

        sr.color = new Color(0.85f, 0.21f, 0.21f, 1);

        yield return new WaitForSeconds(0.2f);

        sr.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(0.2f);

        sr.color = new Color(0.85f, 0.21f, 0.21f, 1);

        yield return new WaitForSeconds(0.2f);

        sr.color = new Color(1, 1, 1, 1);

    }

    IEnumerator Desvio()
    {
        animator.SetBool("Desvio", true);

        desvioEmAndamento = true;

        rigidbox.size = new Vector2(rigidbox.size.x, rigidbox.size.y - 0.3f);

        for(int i = 0; i < 20; i++)
        {   
            if(!olhandoEsquerda)
                transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y);
            
            if(olhandoEsquerda)
                transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.4f);     /*Usando o velocity para mover o personagem*/

        rigidbox.size = new Vector2(rigidbox.size.x, rigidbox.size.y + 0.3f);


        desvioEmAndamento = false;
        animator.SetBool("Desvio", false);

    }

    public void tocarSomPassos()
    {
        somPasso.Play();
    }
    public void tocarSomDano()
    {
        somDano.Play();
    }
    public void tocarSomAtaque()
    {
        somAtaque.Play();
    }
    public void tocarSomChifreColetavel()
    {
        somChifreColetavel.Play();
    }
}