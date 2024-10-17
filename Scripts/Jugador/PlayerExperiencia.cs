using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperiencia : MonoBehaviour
{
    [SerializeField] Slider slider;
    
    private float experienciaActualJugador;

    PlayerVida playerVida;
    SonidoEfecto subirNivelMusica;
    

    void Awake(){
        GetExperienciaActual();
        playerVida = GameObject.Find("VidaExperiencia").GetComponent<PlayerVida>(); 
        subirNivelMusica = GameObject.Find("SubirNivel").GetComponent<SonidoEfecto>(); 
    }

    private void GetExperienciaActual(){

        experienciaActualJugador = GameObject.Find("SliderExperiencia").GetComponent<Slider>().value;
    }

    public void SetExperienciaMaxima(int experienciaMaxima){

        slider.maxValue = experienciaMaxima;
    }

    public void SetExperienciaMinima(int experienciaMinima){

        slider.minValue = experienciaMinima;
    }

    public void SetExperienciaActual(float experienciaActual){

        slider.value = experienciaActual;
    }

    public void AumentarExperienciaActual(float experienciaActual){

        slider.value += experienciaActual;

        if(slider.value >= slider.maxValue){
            SetExperienciaActual(0f);
            subirNivelMusica.Sonido();
            playerVida.SetVidaMaxima( (int) playerVida.GetVidaMaxima() + 1 );
            
            playerVida.AumentarReducirVidaActual(playerVida.GetVidaMaxima());
        }
    }
    
}
