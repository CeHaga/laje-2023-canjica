using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transicao_Fases : MonoBehaviour
{
    public Animator anim;
    public float tempoTransicao = 1;

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>() != null)
            Debug.Log(Physics2D.IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("ProxFase").GetComponent<BoxCollider2D>()));
    }
    public void carregarProximaCena()
    {
        StartCoroutine(carregarLevel(SceneManager.GetActiveScene().buildIndex + 1));    /*Chamando a co-rotina que carrega a próxima cena*/
    }

    private IEnumerator carregarLevel(int index)
    {
        if(index != 0)
        {
            anim.SetTrigger("transicionar");     /*Ativando a animação de transição entre telas*/
            yield return new WaitForSeconds(tempoTransicao);
            SceneManager.LoadScene(index);
            Physics2D.IsTouching(GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>(), GameObject.FindGameObjectWithTag("ProxFase").GetComponent<BoxCollider2D>());
        }
    }
}
