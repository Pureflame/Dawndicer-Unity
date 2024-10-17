using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaFondo : MonoBehaviour
{
    [SerializeField] AudioClip musicaFondoMundo;
    [SerializeField] AudioClip musicaFondoBatalla;

    AudioSource audioFondo;
    // Start is called before the first frame update
    void Awake()
    {
        audioFondo = this.GetComponent<AudioSource>();
    }

    void Start()
    {
        audioFondo.clip = musicaFondoMundo;
        audioFondo.pitch = 1;
        audioFondo.Play();
    }

    public void PararMusica(){
        audioFondo.pitch = 0;
    }

    public void ReactivarMusica(){
        audioFondo.pitch = 1;
    }

    public void CancionJefe(){
        audioFondo.clip = musicaFondoBatalla;
        audioFondo.Play();
    }
}
