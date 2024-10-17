using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVida : MonoBehaviour
{

    public static float tiempoInvencibilidad = 0f;

    public float vidaActualJugador;

    [SerializeField] Slider slider;

    PlayerMovimiento playerMovimiento;

    void Awake(){
        GetVidaActual();
        playerMovimiento = GameObject.Find("HeroKnight").GetComponent<PlayerMovimiento>();
    }

    void Update() {
        tiempoInvencibilidad -= Time.deltaTime;
    }

    public float GetVidaActual(){
        
        return vidaActualJugador = GameObject.Find("SliderSalud").GetComponent<Slider>().value;
    }

    public void SetVidaMaxima(int vidaMaxima){

        slider.maxValue = vidaMaxima;
    }

    public float GetVidaMaxima(){
        return GameObject.Find("SliderSalud").GetComponent<Slider>().maxValue;
    }

    public void SetVidaMinima(int vidaMinima){

        slider.minValue = vidaMinima;
    }

    public void SetVidaActual(float vidaActual){

        slider.value = vidaActual;
    }

    // Cura o hiere al jugador.
    public void AumentarReducirVidaActual(float vidaActual){

        GetVidaActual();
        //Debug.Log("VIDA ACTUAL = " + vidaActualJugador);

        if(tiempoInvencibilidad >= 0f && vidaActual < 0){
            //Debug.Log("INVENCIBILIDAD PROTEGE ATAQUE");

        } else {
            
            float valorAntiguo = slider.value;
            slider.value += vidaActual;

            if(slider.value <= slider.minValue){

                SetVidaActual(slider.minValue);
            } 

            if(slider.value >= slider.maxValue){

                SetVidaActual(slider.maxValue);
            }

            // Se comprueba si se le ha herido.
            if(valorAntiguo >= slider.value){
                
                playerMovimiento.Herido();
                tiempoInvencibilidad = 2f;
            }
        }


    }

}
