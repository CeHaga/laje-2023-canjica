using System.Collections;
using UnityEngine;

public class Boss_3 : MonoBehaviour
{
    public Animator anim;

    private float tempoMinimoEntreAtaques=1.5f, tempoMaximoEntreAtaques = 5.5f;
    public double danoPercentual = 0, escalaPercentual = 0, transformPercentual = 0;
    public float  vida = 36;
    private bool isParado = false, podeAtacarNovamente=true;
    private int cont = 0;
    public int dano = 10;
    public GameObject Espinho1, Espinho2, Espinho3, Espinho4, Espinho5, Espinho6;
    public SpriteRenderer cor;
    
    public CordeiroScriptBoss cordeiro;

    public AudioSource somDano, somNascendo;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        StartCoroutine(comecar());
    }

    void Update()
    {
        if (isParado && cordeiro.ativo)
        {
            if (podeAtacarNovamente)
            {
                StopAllCoroutines();
                StartCoroutine(atacar());
                Debug.Log("alaoalao");
            }
            else
                StopCoroutine(atacar());

            if (Input.GetKeyDown(KeyCode.C))
            {
                cont++;
                isParado = false;
            }
        }
        if(cont == 4)
        {
            anim.SetBool("morte", true);
            isParado = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
   {
        if(other.tag == "AtaquePlayer")
        {
            recebeDano();
        }
   }

    private IEnumerator comecar()
    {
        cordeiro.ativo = false;  /*Destaivando os controles do personagem*/
        yield return new WaitForSeconds(7);
        cordeiro.ativo = true;  /*Reativando os controles do personagem*/
        isParado = true;
    }

    private IEnumerator atacar()
    {
        podeAtacarNovamente = false;
        yield return new WaitForSeconds(Random.Range(tempoMinimoEntreAtaques, tempoMaximoEntreAtaques));
        if (isParado)
        {
            anim.SetBool("ataque", true);
            isParado = false;
        }
        else
            podeAtacarNovamente = true;
        

        Espinho1.transform.position = new Vector2(Random.Range(-9f, 7.31f), Espinho1.transform.position.y);
        Espinho1.SetActive(true);        
        Espinho2.transform.position = new Vector2(Random.Range(-9f, 7.31f), Espinho1.transform.position.y);
        Espinho2.SetActive(true);  
        Espinho3.transform.position = new Vector2(Random.Range(-9f, 7.31f), Espinho1.transform.position.y);
        Espinho3.SetActive(true);       
        Espinho4.transform.position = new Vector2(Random.Range(-9f, 7.31f), Espinho1.transform.position.y);
        Espinho4.SetActive(true);
        Espinho5.transform.position = new Vector2(Random.Range(-9f, 7.31f), Espinho1.transform.position.y);
        Espinho5.SetActive(true);       
        Espinho6.transform.position = new Vector2(Random.Range(-9f, 7.31f), Espinho1.transform.position.y);
        Espinho6.SetActive(true);

    }

    private void terminarAtaque()
    {
        Debug.Log("Ataque acabou!");
        anim.SetBool("ataque", false);
        isParado = true;
        podeAtacarNovamente = true;

    }

    private void terminarDano()
    {
        Debug.Log("Dano acabou!");
        isParado = true;
        anim.SetBool("dano", false);
    }

    private void terminarMorte()
    {
        Debug.Log("Morte acabou!");
        Destroy(gameObject);
        GameObject.FindGameObjectWithTag("ProxFase").SetActive(true);     /*"Spawnando o objeto que far� o jogador zerar o jogo"*/
    }

    void recebeDano()    /*Função chamada ao receber dano*/
    {
        if(danoPercentual == 0)
            danoPercentual = cordeiro.dano/vida;
        if(escalaPercentual == 0)
            escalaPercentual = cor.transform.localScale.x * danoPercentual;
        if(transformPercentual == 0)
            transformPercentual = cor.transform.position.x * (danoPercentual * 10);


        cor.transform.localScale = new Vector3(cor.transform.localScale.x - (float)escalaPercentual, cor.transform.localScale.y);
        cor.transform.position = new Vector2(cor.transform.position.x + (float)transformPercentual, cor.transform.position.y);

        vida -= cordeiro.dano;
        anim.SetBool("dano", true);
        isParado = false;
        tocarSomDano();

        if (vida <= 0) /*Verfica se o personagem perdeu toda sua vida*/
        {
            vida = 0;
            cor.transform.localScale = new Vector3(0, 0, 0);
            anim.SetBool("morte", true);
            Debug.Log("inimigo mrreu");
        }
    }

    public void tocarSomDano()
    {
        somDano.Play();
    }
}