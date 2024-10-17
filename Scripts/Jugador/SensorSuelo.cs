using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorSuelo : MonoBehaviour
{
    private int contador = 0;
    private float tiempoDeshabilitado = 0f;
    private bool suelo;

    // Comprobamos si estamos tocando o no el suelo
    public bool comprobarSuelo() {

        if (tiempoDeshabilitado > 0) {
            return false;
        }

        if(contador > 0){
            suelo = true;
        } else {
            suelo = false;
        }

        return suelo;

    }

    // Se detecta suelo
    void OnCollisionEnter2D(Collision2D other) {
        contador++;
        //Debug.Log("colision del sensor suelo");
    }

    // Se deja de detectar suelo
    void OnCollisionExit2D(Collision2D other) {
        contador--;
    }

    // Detenemos la deteccion de suelo durante un tiempo para evitar problemas (salto, por ejemplo)
    public void Deshabilitar(float tiempo) {
        tiempoDeshabilitado = tiempo;
    }

    // Reducimos el tiempo que está deshabilitado el sensor del suelo.
    void Update() {
        tiempoDeshabilitado -= Time.deltaTime;
    }
}
