using System.Collections;
using UnityEngine;

public class Inimigo : MonoBehaviour
{

    public GameObject vidaVerde, vidaVermelha;
    //public Animator animator;
    public SpriteRenderer cor;
    public BoxCollider2D hitbox;
    public CapsuleCollider2D visãoInimigo;
    public CordeiroScript cordeiro;

    public bool ataqueEmAndamento = false, visão = false, hitboxRange = false, olhandoEsquerda = false, ataquePlayer = false;
    public int danoPercentual = 0, vida = 1, dano = 10;
    private float corPercentual = 1;

    void Update()
    {

        if (vida <= 0) /*Reduz a escala do GameObject á 0 quando a vida chega a zero*/
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.28f, transform.localScale.y - 0.28f, transform.localScale.z - 0.28f);

        }

        if (!ataqueEmAndamento && hitboxRange == true)
            StartCoroutine(Attack());

        if (visão == true)
            perseguir();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "AtaquePlayer" && ataquePlayer)
            recebeDano();
    }

    public void recebeDano()    /*Função chamada ao receber dano*/
    {
        if (danoPercentual == 0)
            danoPercentual = (cordeiro.dano / vida);

        corPercentual -= danoPercentual;

        cor.color = new Color(0, 1, 0.12f, corPercentual);
        vida -= cordeiro.dano;

        if (vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            Debug.Log("inimigo mrreu");
        }
    }

    IEnumerator Attack()
    {

        //animator.SetBool("Ataque", true);

        yield return new WaitForSeconds(0.2f); /* Aguardar até a animação do hit efetivamente aconteça.*/

        //if (animator.GetBool("Ataque"))
        //{
        //    ataqueEmAndamento = true;
        //    hitbox.enabled = true;
        //}

        yield return new WaitForSeconds(0.18f); /* Aguardar até a animação do hit efetivamente aconteça.*/

        hitbox.enabled = false;
        //animator.SetBool("Ataque", false);

        yield return new WaitForSeconds(0.8f);

        ataqueEmAndamento = false;
    }

    void perseguir()
    {
        if (cordeiro.transform.position.x > transform.position.x)
        {
            if (olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = false;
            }
            transform.position = new Vector2(transform.position.x + 0.2f * Time.deltaTime, transform.position.y);
        }
        else if (cordeiro.transform.position.x < transform.position.x)
        {
            if (!olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = true;
            }
            transform.position = new Vector2(transform.position.x - 0.2f * Time.deltaTime, transform.position.y);
        }
    }

}