using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambScript : MonoBehaviour
{   
    private int inputX, velocidade = 60, flip = 0, altura = 0;
    private float distancia;
    public BoxCollider2D hitbox1;
    public Animator animator;
    private Vector2 vector2;
    public GameObject chão;
    private bool pulo = false;

    void Awake(){
        hitbox1 = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update(){
        distancia = (int)transform.position.y - (int)chão.transform.position.y;
        movimento();
    }
    
    void movimento(){
        if(Input.GetKey(KeyCode.Space) && (distancia < 40) && (!pulo)){
            altura += 10;
        }else if((distancia <= 25)){
            altura = 0;
            pulo = false;
        }else if((distancia >= 40)){
            altura -= 5;
            pulo = true;
        }

        if(Input.GetKey("left")){
            if(flip == 0){
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                flip = 1;
            }
            inputX = -1;
            velocidade = 60;
            if(Input.GetKey(KeyCode.LeftShift)){
                velocidade = 150;
            }else{
                velocidade = 60;
            }

        }else if(Input.GetKey("right")){
            if(flip == 1){
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                flip = 0;
            }
            inputX = 1;
            velocidade = 2;
            if(Input.GetKey(KeyCode.LeftShift)){
                velocidade = 150;
            }else{
                velocidade = 60;
            }

        }else{
            velocidade = 0;
            inputX = 0;

        }

        if(Input.GetKey(KeyCode.Z)){
            hitbox1.enabled = true;
        }else{
            hitbox1.enabled = false;
        }

        animator.SetFloat("Velocidade", velocidade);

        transform.position = new Vector2(transform.position.x + inputX * velocidade * Time.deltaTime, transform.position.y + altura * Time.deltaTime);
        vector2 = transform.position;
    }

}

