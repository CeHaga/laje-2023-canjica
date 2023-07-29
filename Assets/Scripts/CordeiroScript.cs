using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CordeiroScript : MonoBehaviour
{
    public int inputX, dano = 5, velocidade = 60, velocidadePulo = 5;
    public float vida = 80, danoPercentual = 0;
    private float distancia, altura = 0;
    private bool pulo = false, olhandoEsquerda=false, ataqueEmAndamento = false, desvioEmAndamento = false, knockback = false;
    public BoxCollider2D hitbox;
    public CapsuleCollider2D hurtbox;
    public Animator animator;
    public SpriteRenderer sprite;
    public Enemy inimigo;
    private Vector3 distanciaKnockback, salvaPosicao;
    public GameObject chao;

    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();

    }

    void Update()
    {   
        if(vida > 0){
            distancia = (int)transform.position.y - (int)chao.transform.position.y;

            movimento();
            if(Input.GetKey("z") && !ataqueEmAndamento)
            {
                StartCoroutine(ataque());
            }

            if(Input.GetKey("x") && !desvioEmAndamento)
            {
                StartCoroutine(desvio());
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        Debug.Log("Entrada:" + other.tag);

        if(other.tag == "AtaqueInimigo")
        {
            StartCoroutine(recebeDano());
            Debug.Log("Tomei dano ai");
        }

        if(other.tag == "VisãoInimigo"){
            inimigo.visão = true;
        }

        if(other.tag == "HitboxRange"){
            inimigo.hitboxRange = true;
            inimigo.visão = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        Debug.Log("Saída:" + other.tag);

        if(other.tag == "VisãoInimigo"){
            inimigo.visão = false;
        }

        if(other.tag == "HitboxRange"){
            inimigo.hitboxRange = false;
            inimigo.visão = true;
        }
    }

    void movimento()
    {

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
    
    }
    IEnumerator ataque()
    {   
            animator.SetBool("Ataque", true);

            yield return new WaitForSeconds(0.2f); /* Aguardar até a animação do hit efetivamente aconteça.*/

            if(animator.GetBool("Ataque"))
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

        if(danoPercentual == 0)
        {
            danoPercentual = (inimigo.dano/vida);
        }

        vida -= inimigo.dano;

        if(vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            animator.SetBool("Vida", false);

            yield return new WaitForSeconds(0.6f);

            transform.position = new Vector2(transform.position.x, transform.position.y - 12f);
        }
    
        //Mudança de cor ao tomar dano

        sprite.color = new Color (0.85f, 0.21f, 0.21f, 1);

        yield return new WaitForSeconds(0.2f);
       
        sprite.color = new Color (1, 1, 1, 1);
        
        yield return new WaitForSeconds(0.2f);

        sprite.color = new Color (0.85f, 0.21f, 0.21f, 1);

        yield return new WaitForSeconds(0.2f);

        sprite.color = new Color (1, 1, 1, 1);

    }
    IEnumerator desvio(){
        animator.SetBool("Desvio", true);

        desvioEmAndamento = true;
        hurtbox.enabled = false;

        yield return new WaitForSeconds(0.4f);
            
        hurtbox.enabled = true;
        desvioEmAndamento = false;
        animator.SetBool("Desvio", false);

    }

}