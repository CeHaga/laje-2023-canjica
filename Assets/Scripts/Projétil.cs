using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projétil : MonoBehaviour
{
        public GameObject projetil;
    // Start is called before the first frame update
    void Start()
    {
       projetil = new GameObject("Projétil");

       projetil.AddComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
