using System.Collections;
using UnityEngine;

public class CordeiroAcordando : MonoBehaviour
{
    public GameObject cordeiro;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(comecarAnimacao());
    }

    private void subirPersonagem1()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 6, transform.position.z);     /*Subindo um pouco o personagem do cordeiro acordando*/
    }

    private void subirPersonagem2()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);      /*Subindo mais um pouco o personagem do cordeiro acordando*/
    }

    private IEnumerator comecarAnimacao()
    {
        yield return new WaitForSeconds(1.5f);    /*Esperando um tempo antes de começar a animação*/
        anim.SetBool("acordar", true);
    }

    private void terminarAnimacao()
    {
        Destroy(gameObject);     /*Destruindo o objeto do cordeiro acordando*/
        cordeiro.SetActive(true);    /*Fazendo o personagem principal aparecer*/
    }
}