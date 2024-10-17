using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogoIndividual : MonoBehaviour
{

    public static bool estaHablando = false;

    public DialogoDatos dialogo;

    [SerializeField] GameObject bocadilloDialogo;

    private bool estaDentro = false;
    private float tiempoDeshabilitado = 0f;
    private LayerMask capaJugador;
    private LayerMask capaNPC;
    
    SonidoEfecto hablarNPCSonido;

    void Awake(){

        capaJugador = LayerMask.NameToLayer("Jugador"); 
        capaNPC = LayerMask.NameToLayer("NPC"); 
        hablarNPCSonido = GameObject.Find("HablarNPC").GetComponent<SonidoEfecto>();
    }

    void Start(){

        this.transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update(){

        EstaDentro();
        tiempoDeshabilitado -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {

        //Debug.Log("DENTRO NPC");
        if( other.gameObject.tag == "Player"){
                estaDentro = true;
                this.transform.GetChild(0).gameObject.SetActive(true);
        }
        
    }

    void OnTriggerExit2D(Collider2D other) {
        
        //Debug.Log("FUERA NPC");
        if( other.gameObject.tag == "Player"){
            estaDentro = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
        
    }

    void EstaDentro(){

        // Inicia la conversacion con los datos que le pasamos. Se ejecuta una vez.
        if(Input.GetKey(KeyCode.E) && estaDentro == true && estaHablando == false && tiempoDeshabilitado <= 0){
            
            hablarNPCSonido.Sonido();

            GameObject.Find("Canvas").GetComponent<DialogoController>().IniciarDialogo(dialogo);
            estaHablando = true;
            tiempoDeshabilitado = 1f;
        }

        // Continua la conversacion. Se ejecuta el resto de veces.
        if( (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C) || Input.GetKey(KeyCode.Space)) && estaDentro == true && estaHablando == true && tiempoDeshabilitado <= 0){
            
            GameObject.Find("Canvas").GetComponent<DialogoController>().SiguienteFrase();
            tiempoDeshabilitado = 1f;
        }
    }
}

// Se escribe aqui el dialogo a utilizar por la clase "DialogoDatos".
// Se podria escribir en un Script diferente, pero resulta más cómodo así en mi opinión.
[System.Serializable] public class DialogoDatos{

    public string nombrePersonaje;

    [TextArea(1, 5)]
    public string[] frasesDialogo;

}