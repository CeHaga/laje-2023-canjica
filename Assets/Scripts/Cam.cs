using UnityEngine;
using UnityEngine.SceneManagement;

public class Cam : MonoBehaviour
{
    private Transform player;
    private Vector3 seguindo;
    public GameObject paredeInvisivel1, paredeInvisivel2;
    private float posicaoInicialX= 0, posicaoFinalX;      /*Aqui devem ser definidas as posi��es X m�nimas e m�ximas do personagem em cada fase*/

    private bool isCorredor=false, paredeInvisivel1Destruida = false, paredeInvisivel2Destruida = false;
    public static int numInimigosDerrotados = 0;    /*Esta vari�vel precisa ser incrementada a cada vez que um inimigo for derrotado*/
    private int distanciaInicialPersonagem=5;     /*Esta vari�vel define em que posi��o o personagem estar� na c�mera*/

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
        /*Fazendo a movimenta��o da c�mera*/
        if(isCorredor)     /*A movimenta��o da c�mera s� vai ocorrer no corredor*/
        {
            /*os valores a seguir ir�o mudar de acordo com o tamanho da fase e da posi��o das paredes invis�veis*/
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
                seguindo = new Vector3(player.position.x + distanciaInicialPersonagem, transform.position.y, transform.position.z);    /*Esta vari�vel guardar� a posi��o do personagem apenas no eixo x*/
                transform.position = Vector3.Lerp(transform.position, seguindo, 10 * Time.deltaTime);     /*Aqui � definido o que a camer� ir� seguir e com qual suavidade*/
            }

            /*Destruindo as paredes invis�veis*/
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
