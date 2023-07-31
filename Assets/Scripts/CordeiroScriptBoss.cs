using System.Collections;
using UnityEngine;

public class CordeiroScriptBoss : MonoBehaviour
{
    public float dano = 7;
    private float vida = 100, timerPulo, velocidadeAtual;
    private bool olhandoEsquerda = false, ataqueEmAndamento = false, desvioEmAndamento = false, isPulando, releasedJump = true;
    public BoxCollider2D hitbox, hurtbox, rigidbox;

    private Animator animator;
    public Boss_3 inimigo;
    private Rigidbody2D rig;
    private SpriteRenderer sr;

    /*Definindo as constantes do movimento*/
    private const float VEL_ANDANDO = 1.5f, VEL_CORRENDO = 3, FORCA_PULO = 1;
    private const float TEMPO_PULO = 0.25f; /*Este ser� o tempo que o usu�rio ter� que segurar o bot�o de pulo para que o personagem atinja sua altura m�xima*/

    public bool ativo = true; /*Esta vari�vel ser� usada para desativar os controles do personagem em certos pontos do jogo*/

    public AudioSource somAtaque, somPasso, somDano, somChifre;
    public float danoPercentual=0, escalaPercentual=0, transformPercentual=0;
    public RectTransform barraVerde;


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
                StartCoroutine(Desvio());

            if (Input.GetKeyDown(KeyCode.LeftControl))
                Cam.numInimigosDerrotados++;
        }
        else if(vida <= 0)
        {
            rigidbox.size = new Vector2(hurtbox.size.x, hurtbox.size.y - 0.6f);
        }
    }

   private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.tag == "Ground") /*Verificando se o personagem est� em contato com algum gameObject com a tag "Ground"*/
        {
            isPulando = false;
            animator.SetBool("NoAr", false);
        }

        if(colisao.gameObject.tag == "Inimigo")
            StartCoroutine(recebeDano(inimigo.dano));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Foguinho" && !desvioEmAndamento && inimigo.vida > 0)
            StartCoroutine(recebeDano(5));
        if (other.gameObject.tag == "ProxFase") /*Verificando se o personagem encostou no trigger que o leva para a pr�xima fase*/
        {
            Debug.Log("Passou de fase");
            Transicao_Fases.transicao = true; /*Chamando a fun��o de carregar a pr�xima cena*/
        }
        if (other.gameObject.tag == "Chifre") /*Verificando se o personagem encostou no trigger que o leva para a pr�xima fase*/
        {
            tocarSomChifreColetavel();
            Destroy(other.gameObject);
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

    IEnumerator recebeDano(int dano) /*Fun��o chamada ao receber dano*/
    {
        Debug.Log(vida);
        tocarSomDano();
        if (danoPercentual == 0)
            danoPercentual = (inimigo.dano / vida);
        if (escalaPercentual == 0)
            escalaPercentual = barraVerde.localScale.x * danoPercentual;
        if (transformPercentual == 0)
            transformPercentual = barraVerde.position.x * danoPercentual;

        barraVerde.localScale = new Vector3(barraVerde.localScale.x - (float)escalaPercentual, barraVerde.localScale.y);
        barraVerde.position = new Vector2(barraVerde.position.x - (float)transformPercentual, barraVerde.position.y);

        vida -= dano;
        tocarSomDano();

        if (vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            ativo = false;
            animator.SetBool("Vida", false);
            GameController.morreu = true;
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

        rigidbox.size = new Vector2(hurtbox.size.x, hurtbox.size.y - 0.3f);

        if(!olhandoEsquerda)
            rig.velocity = new Vector2(1 * 70f, rig.velocity.y);
        else
            rig.velocity = new Vector2(-1 * 70f, rig.velocity.y);     /*Usando o velocity para mover o personagem*/

        yield return new WaitForSeconds(0.8f);     /*Usando o velocity para mover o personagem*/

        rigidbox.size = new Vector2(hurtbox.size.x, hurtbox.size.y + 0.3f);

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
        somChifre.Play();
    }

}