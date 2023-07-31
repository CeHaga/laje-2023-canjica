using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour
{
    private Transform player;
    private Vector3 seguindo;
    public GameObject paredeInvisivel1, paredeInvisivel2;
    private float posicaoInicialX= 0, posicaoFinalX;      /*Aqui devem ser definidas as posições X mínimas e máximas do personagem em cada fase*/

    private bool isCorredor=false, paredeInvisivel1Destruida = false, paredeInvisivel2Destruida = false;
    public static int numInimigosDerrotados = 0;    /*Esta variável precisa ser incrementada a cada vez que um inimigo for derrotado*/
    private int distanciaInicialPersonagem=5;     /*Esta variável define em que posição o personagem estará na câmera*/

    public int contTeste = 0;

    private void Start()
    {
        string nomeCena = SceneManager.GetActiveScene().name;
        if (nomeCena.Contains("Corredor"))     /*Se estiver na parte do corredor*/
            isCorredor = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;    /*Aqui eu procuro o gameobject com a tag "Player"*/
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            contTeste++;
        }
    }

    void FixedUpdate()
    {
        /*Fazendo a movimentação da câmera*/
        if(isCorredor)     /*A movimentação da câmera só vai ocorrer no corredor*/
        {
            /*os valores a seguir irão mudar de acordo com o tamanho da fase e da posição das paredes invisíveis*/
            if (numInimigosDerrotados < 2 && contTeste < 2)
                posicaoFinalX = 9.2f - distanciaInicialPersonagem;
            else if (numInimigosDerrotados < 4 && contTeste < 4)
            {
                posicaoFinalX = 27.5f - distanciaInicialPersonagem;
                Debug.Log("aaaa");
            }
            else
                posicaoFinalX = 40.2f - distanciaInicialPersonagem;

            if (player.position.x >= posicaoInicialX && player.position.x <= posicaoFinalX)
            {
                seguindo = new Vector3(player.position.x + distanciaInicialPersonagem, transform.position.y, transform.position.z);    /*Esta variável guardará a posição do personagem apenas no eixo x*/
                transform.position = Vector3.Lerp(transform.position, seguindo, 10 * Time.deltaTime);     /*Aqui é definido o que a camerá irá seguir e com qual suavidade*/
            }

            /*Destruindo as paredes invisíveis*/
            if ((numInimigosDerrotados == 2 || contTeste == 2) && !paredeInvisivel1Destruida)
            {
                Destroy(paredeInvisivel1);
                paredeInvisivel1Destruida = true;
            }
            if ((numInimigosDerrotados == 4 || contTeste == 4) && !paredeInvisivel2Destruida)
            {
                Destroy(paredeInvisivel2);
                paredeInvisivel2Destruida = true;
            }
        }
    }
}
