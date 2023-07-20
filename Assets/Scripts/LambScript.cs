using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LambScript : MonoBehaviour
{   
    public Rigidbody2D esseCorpoRigido;
    private int inputX, inputY;
    public int pulo, velocidade, altura;
    private SpriteRenderer spriteRenderer;
    public Animator animator;

    void Awake(){
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update(){
        movimento();
    }
    
    void movimento(){

        if(Input.GetKey("left")){
            spriteRenderer.flipX = false;
            inputX = -1;
            animator.SetBool("Correndo", true);

        }
        else if(Input.GetKey("right")){
            spriteRenderer.flipX = true;
            inputX = 1;
            animator.SetBool("Correndo", true);

        }else{
            inputX = 0;
            animator.SetBool("Correndo", false);

        }
        if(Input.GetKey(KeyCode.Space)){
            inputY = pulo;
            animator.SetBool("Correndo", false);

        }else{
            inputY = 0;
        }

       transform.position = new Vector2(transform.position.x + inputX * velocidade * Time.deltaTime, transform.position.y + inputY * altura * Time.deltaTime);
    }

}

