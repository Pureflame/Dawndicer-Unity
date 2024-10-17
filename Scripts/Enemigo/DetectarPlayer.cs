using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectarPlayer : MonoBehaviour
{

    [HideInInspector] public bool detectadoPlayer = false;

    // El jugador ha entrado en rango de ataque del enemigo
    void OnTriggerEnter2D(Collider2D other) {
        
        if( other.gameObject.tag == "Player"){
            detectadoPlayer = true;
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        
        if( other.gameObject.tag == "Player"){
            detectadoPlayer = true;
        }
    }

    // El jugador ha salido en rango de ataque del enemigo
    void OnTriggerExit2D(Collider2D other) {
        
        if( other.gameObject.tag == "Player"){
            detectadoPlayer = false;
        }
    }

}
