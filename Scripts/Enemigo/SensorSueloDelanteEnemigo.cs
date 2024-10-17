using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSueloDelanteEnemigo : MonoBehaviour
{
    private int contador = 0;
    private bool sueloEnemigo;

    // Comprobamos si estamos tocando o no el suelo
    public bool comprobarSuelo() {

        if(contador > 0){
            sueloEnemigo = true;
        } else {
            sueloEnemigo = false;
        }

        return sueloEnemigo;
    }

    // Se detecta suelo
    void OnCollisionEnter2D(Collision2D other) {
        contador++;
    }

    // Se deja de detectar suelo
    void OnCollisionExit2D(Collision2D other) {
        contador--;
    }
}
