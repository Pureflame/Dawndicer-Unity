using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoEfecto : MonoBehaviour
{
    [SerializeField] AudioClip sonido;
    AudioSource audioSource;
    
    void Awake()
    {
        audioSource = this.GetComponent<AudioSource>(); 
    }

    void Start(){
        audioSource.clip = sonido;
    }

    public void Sonido(){
        audioSource.Play();
    }
}
