using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinchos : MonoBehaviour
{

    private bool estaDentroPinchos;
    PlayerVida playerVida;

    void Awake(){
        playerVida = GameObject.Find("VidaExperiencia").GetComponent<PlayerVida>();
    }

    void Start() {
        estaDentroPinchos = false;
    }

    void Update(){
        HeridaConstantePinchos();
    }

    // El jugador ha tocado al enemigo y recibe daño por ello.
    void OnTriggerEnter2D(Collider2D other) {
        
        if( other.gameObject.tag == "Player" && PlayerVida.tiempoInvencibilidad <= 0 && PlayerMovimiento.muerteJugador == false){
                estaDentroPinchos = true;
                playerVida.AumentarReducirVidaActual(-2);
                
        }
    }


    // El jugador ya no toca al enemigo.
    // Por tanto, ya no recibe daño constante.
    void OnTriggerExit2D(Collider2D other) {
       
        if( other.gameObject.tag == "Player"){
            estaDentroPinchos = false;
        }
        
    }

    public void HeridaConstantePinchos(){
        if(estaDentroPinchos == true && PlayerVida.tiempoInvencibilidad <= 0 && PlayerMovimiento.muerteJugador == false){

            playerVida.AumentarReducirVidaActual(-2);

        }
    }
}
