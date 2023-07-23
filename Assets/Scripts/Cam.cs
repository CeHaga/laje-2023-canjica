using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private Transform player;
    private Vector3 seguindo;
    private int posicaoInicialX=-225, posicaoFinalX=277;      /*Aqui devem ser definidas as posições X mínimas e máximas do personagem em cada fase*/

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;    /*Aqui eu procuro o gameobject com a tag "Player"*/
    }

    void FixedUpdate()
    {
        if(player.position.x >= posicaoInicialX && player.position.x <= posicaoFinalX)
        {
            seguindo = new Vector3(player.position.x, transform.position.y, transform.position.z);    /*Esta variável guardará a posição do personagem apenas no eixo x*/
            transform.position = Vector3.Lerp(transform.position, seguindo, 10 * Time.deltaTime);     /*Aqui é definido o que a camerá irá seguir e com qual suavidade*/
        }
    }
}
