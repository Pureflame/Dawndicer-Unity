using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaController : MonoBehaviour
{

    
    [SerializeField] float tiempoEspera;

    private bool tocandoElSuelo;
    private bool volverABajar = true;
    private PlatformEffector2D platformEffector2D;

    PlayerMovimiento playerMovimiento;
    

    void Awake() {
        platformEffector2D = GetComponent<PlatformEffector2D>();
        playerMovimiento = GameObject.Find("HeroKnight").GetComponent<PlayerMovimiento>();
    }


    void Update()
    {

        if(MenuPausaController.juegoPausado == false){

            tocandoElSuelo = playerMovimiento.tocoSuelo;

            // Esperar un tiempo antes de poder tocar otra plataforma y bajar de ella (si hay varias juntas verticalmente las atraviesa)
            if(tiempoEspera >= 0){
                tiempoEspera -= Time.deltaTime;

                if(tiempoEspera <= 0 && volverABajar == false){
                    volverABajar = true;
                    platformEffector2D.rotationalOffset = 0f;
                    //Debug.Log("otra vez puedes");
                }
            }

            // Bajar Plataformas al pulsar la flecha hacia abajo
            if(Input.GetKeyDown(KeyCode.DownArrow) && tocandoElSuelo == true) {

                if(tiempoEspera <= 0){
                    //Debug.Log("bajamos");
                    playerMovimiento.animaciones.SetTrigger("Caida");
                    this.platformEffector2D.rotationalOffset = 180f;
                    tiempoEspera = 0.4f;
                    volverABajar = false;
                }
            }
        }
    }
}


