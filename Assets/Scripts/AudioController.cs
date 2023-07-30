using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public static bool isTocandoMusica = false;
    public AudioSource musica;

    private void Start()
    {
        Debug.Log(isTocandoMusica);
        if (!isTocandoMusica)
        {
            musica.Play();
            isTocandoMusica = true;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
