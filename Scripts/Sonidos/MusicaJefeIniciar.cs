using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaJefeIniciar : MonoBehaviour
{
    [SerializeField] AudioClip sonido;
    MusicaFondo musicaFondo;

    private bool iniciar = false;
    
    void Awake()
    {
        musicaFondo = GameObject.Find("MusicaFondo").GetComponent<MusicaFondo>();
    }

    // El jugador ya no toca el dado
    void OnTriggerEnter2D(Collider2D other) {
        
        if(iniciar == false){
            musicaFondo.CancionJefe();
            iniciar = true;
        }
    }
}
