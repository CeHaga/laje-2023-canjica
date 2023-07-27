using UnityEngine;

public class CordeiroAcordando : MonoBehaviour
{
    public GameObject cordeiro;

    private void Start()
    {
        Cam.acabouAnimacaoAcordar = false;      /*Definindo a variável como false para que a câmera não tente achar o player por enquanto*/
    }

    private void subirPersonagem1()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 6, transform.position.z);     /*Subindo um pouco o personagem do cordeiro acordando*/
    }

    private void subirPersonagem2()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);      /*Subindo mais um pouco o personagem do cordeiro acordando*/
    }

    private void terminarAnimacao()
    {
        Destroy(gameObject);     /*Destruindo o objeto do cordeiro acordando*/
        cordeiro.SetActive(true);    /*Fazendo o personagem principal aparecer*/
        Cam.acabouAnimacaoAcordar = true;     /*Depois que a animação do cordeiro acordando acabar, a câmera pode tentar achar o player*/
    }
}
