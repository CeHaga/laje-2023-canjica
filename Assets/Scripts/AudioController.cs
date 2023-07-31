using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public static bool isTocandoMusica = false;
    private static AudioController instance;
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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
    }

    public static AudioController GetInstance()
    {
        return instance;
    }

    public void StopAudio()
    {
        if (musica != null && musica.isPlaying)
            musica.Stop();
    }

    public void PlayAudio()
    {
        if (musica != null && !musica.isPlaying)
            musica.Play();
    }
}
