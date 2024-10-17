using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamaraSacudida : MonoBehaviour
{

    public GameObject cinemachine;

    private float tiempoSacudida = 0f;
    private float fuerzaSacudida = 0f;

    void Start() {
        tiempoSacudida = 0f;
        fuerzaSacudida = 2f;
    }

    // La camara se sacude cuando el jugador recibe una herida
    public void SacudirCamara(){
        StartCoroutine("Sacudida");
    }

    IEnumerator Sacudida() {

        CinemachineBasicMultiChannelPerlin cinemachineBasic = cinemachine.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        
        tiempoSacudida = 0.2f;
        cinemachineBasic.m_AmplitudeGain = fuerzaSacudida;

        while(tiempoSacudida > 0){

            tiempoSacudida -= Time.deltaTime;

            yield return null;
        }
        
        cinemachineBasic = cinemachine.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasic.m_AmplitudeGain = 0f;
    }
}
