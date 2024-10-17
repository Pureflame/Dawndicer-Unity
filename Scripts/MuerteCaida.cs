using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuerteCaida : MonoBehaviour
{

    PlayerVida playerVida;

    void Awake(){
        playerVida = GameObject.Find("VidaExperiencia").GetComponent<PlayerVida>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        
        if( other.gameObject.tag == "Player" && PlayerMovimiento.muerteJugador == false){
                playerVida.AumentarReducirVidaActual(-999);
        }
    }
}
