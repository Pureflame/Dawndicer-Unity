using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dadoRecoger : MonoBehaviour
{

    [SerializeField] GameObject dadoActual;
    [SerializeField] GameObject dadoSecundario;
    [SerializeField] Sprite d4;
    [SerializeField] Sprite noDado;

    public bool estaDentro = false;
    private float esperaSubida;
    private float esperaBajada;
    private LayerMask capaJugador;
    private LayerMask capaDado;

    DadoController dadoController;
    SonidoEfecto dadoRecogerMusica;
    
    void Awake(){

        capaJugador = LayerMask.NameToLayer("Jugador"); 
        capaDado = LayerMask.NameToLayer("Dado"); 
        dadoController = GameObject.Find("DadoController").GetComponent<DadoController>();
        dadoRecogerMusica = GameObject.Find("DadoRecogerMusica").GetComponent<SonidoEfecto>(); 
    }

    void Start(){

        esperaSubida = 0f;
        esperaBajada = 1f;
        estaDentro = false;
    }

    void Update(){
        EstaDentro();
        MoverDadoAire();
    }

    // El jugador toca el dado
    void OnTriggerEnter2D(Collider2D other) {
        if( other.gameObject.tag == "Player"){
            estaDentro = true;
        }
    }

    // El jugador ya no toca el dado
    void OnTriggerExit2D(Collider2D other) {
        if( other.gameObject.tag == "Player"){
            estaDentro = false;
        }
    }

    // Animar el dado en el aire haciendo que suba y baje
    void MoverDadoAire(){
        
        if(esperaSubida > 0f){

            esperaSubida -= Time.deltaTime;

            if(esperaSubida <= 0){
                esperaBajada = 1f;
            }

        } else {
            
            DadoSube();
        }

        if(esperaBajada > 0f){

            esperaBajada -= Time.deltaTime;

            if(esperaBajada <= 0){
                esperaSubida = 1f;
            }

        } else {
            
            DadoBaja();
        }
        
    }

    void DadoSube(){

        if(MenuPausaController.juegoPausado == true){

            transform.Translate(new Vector2(0f,0.0f));

        } else {
            
            transform.Translate(new Vector2(0f,0.0005f));
        }
        
        
    }

    void DadoBaja(){
        if(MenuPausaController.juegoPausado == true){

            transform.Translate(new Vector2(0f,0.0f));

        } else {

            transform.Translate(new Vector2(0f,-0.0005f));
        }
        
    }
    

    // El jugador puede recoger el dado si esta dentro del dado.
    // Controlamos esta accion del jugador desde aqui para facilitar 
    // el detectar los dados individualmente.
    void EstaDentro(){

        if(Input.GetKeyDown(KeyCode.C) && estaDentro == true && DialogoIndividual.estaHablando == false){
            
            dadoRecogerMusica.Sonido();

            //StartCoroutine("RecogerDado");

            if(dadoActual.GetComponent<Image>().sprite == d4){

                dadoActual.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                
                //Debug.Log("DADO RECOGIDO, CAMBIA ACTUAL");
                //Destroy(gameObject);
                this.gameObject.SetActive(false);

            } else{

                dadoSecundario.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                
                DadoController.dadoSecundarioLleno = true;
                dadoSecundario.gameObject.SetActive(true);

                
                //Debug.Log("DADO RECOGIDO, CAMBIA SECUNDARIO");
                //Destroy(gameObject);
                this.gameObject.SetActive(false);

            }


            estaDentro = false;
        }

    }

    IEnumerator RecogerDado() {

        if(dadoActual.GetComponent<Image>().sprite == d4){

            dadoActual.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            yield return new WaitForSeconds (0.1f);
            //Debug.Log("DADO RECOGIDO, CAMBIA ACTUAL");
            //Destroy(gameObject);
            this.gameObject.SetActive(false);

        } else{

            dadoSecundario.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            
            DadoController.dadoSecundarioLleno = true;
            dadoSecundario.gameObject.SetActive(true);

            yield return new WaitForSeconds (0.1f);
            //Debug.Log("DADO RECOGIDO, CAMBIA SECUNDARIO");
            //Destroy(gameObject);
            this.gameObject.SetActive(false);

        }

    }

    IEnumerator MoverDado() {
        //Debug.Log("SUBE");
        transform.Translate(new Vector2(0f,0.001f));
        yield return new WaitForSeconds (1f);
        //Debug.Log("BAJA");
        transform.Translate(new Vector2(0f,-0.001f));
        yield return new WaitForSeconds (1f);
    }
}
