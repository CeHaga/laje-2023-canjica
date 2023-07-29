using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject vidaVerde, vidaVermelha;
    public int danoPercentual = 0, vida = 1, dano = 10;
    private float corPercentual = 1;
    public Animator animator;
    public SpriteRenderer cor;
    public BoxCollider2D hitbox;
    public CapsuleCollider2D visãoInimigo;
    public CordeiroScript cordeiro;
    private Vector2 targetPosition;
    public SpriteRenderer sprite;
    public bool ataqueEmAndamento = false, visão = false, hitboxRange = false, olhandoEsquerda=false;

    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
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

        
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        if(other.tag == "AtaquePlayer")
        {
            recebeDano();
        }

        if(other.tag == "Player" && visão == true)
        {
            perseguir();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if(!ataqueEmAndamento && other.tag == "Player" && hitboxRange == true)
        {
            StartCoroutine(Attack());
        }

        if(other.tag == "Player" && visão == true)
        {
            perseguir();
        }
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

        if(vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            animator.SetBool("Vida", false);
        }
    }

    IEnumerator Attack()
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

        yield return new WaitForSeconds(0.8f);
        
        ataqueEmAndamento = false;
    }

    void perseguir()
    {
        if(cordeiro.transform.position.x > transform.position.x)
        {   
            if(!olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = true;
            }
            transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
        }
        else if(cordeiro.transform.position.x < transform.position.x)
        {
           if (olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = false;
            }
            transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y);
        }
    }

}
