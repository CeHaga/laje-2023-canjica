using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject barra_de_vida_verde, barra_de_vida_vermelha;
    private double vida = 10, danoRecebido = 5;

    void Awake(){
        
    }

    void Update(){
    }
    void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Acertou!");
        dano();
        if(vida <= 0){
            barra_de_vida_vermelha.transform.localScale = new Vector3(0, 0, 0);
        }
    }
    void dano(){
        vida -= danoRecebido;
        barra_de_vida_verde.transform.localScale = new Vector3((float)vida / 10, barra_de_vida_verde.transform.localScale.y, barra_de_vida_verde.transform.localScale.z);
    }
}
