using System.Collections;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    //public Animator animator;
    public SpriteRenderer cor;
    public BoxCollider2D hitbox;
    public CapsuleCollider2D visaoInimigo;
    public CordeiroScript cordeiro;

    public bool ataqueEmAndamento = false, visao = false, hitboxRange = false, olhandoEsquerda = false, ataquePlayer = false;
    public float vida = 30;
    public float danoPercentual = 0;
    public float dano = 1.6f;
    private float corPercentual = 1;

    public bool isDandoDano = false;

    public Animator anim;

    void Update()
    {

        if(vida <= 0) /*Reduz a escala do GameObject á 0 quando a vida chega a zero*/
        {
            if(transform.localScale.x >= 0)
                transform.localScale = new Vector3(transform.localScale.x - 0.035f, transform.localScale.y , transform.localScale.z);
            if(transform.localScale.y >= 0)  
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.03f, transform.localScale.z);
            if(transform.localScale.z >= 0)  
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - 0.03f);

            if (transform.localScale.x <= 0 && transform.localScale.y <= 0 && transform.localScale.z <= 0)
            {
                Destroy(gameObject);
                cordeiro.cont++;
                Cam.numInimigosDerrotados += 1;
                if(cordeiro.cont < 5)
                {
                    if (GameObject.FindGameObjectWithTag("Inimigo" + cordeiro.cont) != null)    /*Se ainda tiver inimigos*/
                        cordeiro.inimigo = GameObject.FindGameObjectWithTag("Inimigo" + cordeiro.cont).GetComponent<Inimigo>();
                }
            }

        }
        else
        {
            anim.SetBool("ataque", false);
            anim.SetBool("andando", false);
            if (visao == true)
                perseguir();
            else
            {
                if (!ataqueEmAndamento && hitboxRange == true && !isDandoDano)
                {
                    isDandoDano = true;
                    anim.SetBool("ataque", true);
                    anim.SetBool("andando", false);
                    StartCoroutine(Attack());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "AtaquePlayer" && ataquePlayer == true)
        {
            recebeDano();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "AtaquePlayer" && ataquePlayer == true)
            recebeDano();
    }

    public void recebeDano()    /*Função chamada ao receber dano*/
    {
        if (danoPercentual == 0)
            danoPercentual = (cordeiro.dano / vida);

        corPercentual -= danoPercentual;

        cor.color = new Color(0, 1, 0.12f, corPercentual);
        vida -= cordeiro.dano;

        if (vida <= 0) /*Verifica se o personagem perdeu toda sua vida*/
            vida = 0;
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f); /* Aguardar até a animação do hit efetivamente aconteça.*/
        if(vida > 0)
        {
            ataqueEmAndamento = true;
            hitbox.enabled = true;
        }
        yield return new WaitForSeconds(0.5f); /* Aguardar até a animação do hit efetivamente aconteça.*/
        isDandoDano = false;
        hitbox.enabled = false;
        yield return new WaitForSeconds(0.5f);
        ataqueEmAndamento = false;
    }

    void perseguir()
    {
        anim.SetBool("andando", true);
        anim.SetBool("ataque", false);
        if (cordeiro.transform.position.x > transform.position.x)
        {
            if (olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = false;
            }
            transform.position = new Vector2(transform.position.x + 1.0f * Time.deltaTime, transform.position.y);
        }
        else if (cordeiro.transform.position.x < transform.position.x)
        {
            if (!olhandoEsquerda)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                olhandoEsquerda = true;
            }
            transform.position = new Vector2(transform.position.x - 1.0f * Time.deltaTime, transform.position.y);
        }
    }

}