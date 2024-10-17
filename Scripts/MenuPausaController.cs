using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuPausaController : MonoBehaviour
{

    public static bool juegoPausado = false;

    public GameObject MenuPausa;
    public GameObject cinemachine;

    [SerializeField] AudioClip aceptar;
    [SerializeField] AudioClip moverFlecha;
    //[SerializeField] AudioClip musicaFondoMenu;
    [SerializeField] GameObject botonSalir;
    [SerializeField] GameObject botonVolverMenu;
    [SerializeField] GameObject botonContinuar;
    [SerializeField] GameObject flecha;

    private GameObject seleccionado;
    private int botonSeleccionado;

    AudioSource audioSource;
    AudioSource audioMenu;
    MusicaFondo musicaFondo;

    void Awake(){

        audioSource = GetComponent<AudioSource>();
        audioMenu = GetComponent<AudioSource>();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonContinuar);
        musicaFondo = GameObject.Find("MusicaFondo").GetComponent<MusicaFondo>();

        MenuPausa.SetActive(false);
    }

    void Start() {

        seleccionado = EventSystem.current.currentSelectedGameObject;
        seleccionarBoton();

        //audioMenu.clip = musicaFondoMenu;
        //audioMenu.Play();
    }

    void Update(){

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {

        }

        if( Input.GetKeyDown(KeyCode.Escape) ){

            if(juegoPausado == true) {
                ReanudarPartida();
                musicaFondo.ReactivarMusica();
            }
            else {
                PausarPartida();
                musicaFondo.PararMusica();
            }
        }

        if(juegoPausado == true){

            if(seleccionado != EventSystem.current.currentSelectedGameObject){
                audioSource.clip = moverFlecha;
                audioSource.Play();
            }

            if(EventSystem.current.currentSelectedGameObject == null){
                EventSystem.current.SetSelectedGameObject(seleccionado);
            }
            seleccionado = EventSystem.current.currentSelectedGameObject;

            // Absorber clicks del raton ajenos a los botones
            if(seleccionado != botonContinuar && seleccionado != botonVolverMenu && seleccionado != botonSalir){
                
            } else {
                flecha.transform.position = new Vector2(flecha.transform.position.x, seleccionado.transform.position.y - 15);
                seleccionarBoton();
            }



            if(Input.GetKeyDown(KeyCode.Z)){

                switch (botonSeleccionado){

                    // Jugar
                    case 0:
                        //Debug.Log("Reanudamos el juego");
                        ReanudarPartida();
                        musicaFondo.ReactivarMusica();
                        break;

                    // Controles    
                    case 1:
                        //Debug.Log("Volver al menu");
                        ReanudarPartida();
                        
                        SceneManager.LoadScene(0);
                        break;

                    // Salir
                    case 2:
                        //Debug.Log("Salimos");
                        //Time.timeScale = 1f;
                        SalirJuego();
                        break;

                    default:
                        //Debug.Log("Hay un error en el menu de pausa");
                        break;
                }
            }
        }
    }

    void PausarPartida(){
        cinemachine.SetActive(false);
        audioSource.clip = aceptar;
        audioSource.Play();

        MenuPausa.SetActive(true);
        juegoPausado = true;
        Time.timeScale = 0f;
    }

    // Reanuda la partida. Es público para llamarlo en una de los botones también.
    public void ReanudarPartida(){

        MenuPausa.SetActive(false);
        juegoPausado = false;
        Time.timeScale = 1f;
        cinemachine.SetActive(true);
    }

    public void seleccionarBoton(){
        
        if(seleccionado == botonContinuar){
            botonSeleccionado = 0;
        }
        if(seleccionado == botonVolverMenu){
            botonSeleccionado = 1;
        }
        if(seleccionado == botonSalir){
            botonSeleccionado = 2;
        }

    }

    // Salimos del videojuego
    public void SalirJuego(){

        //StartCoroutine(EsperarParaSalir());
        botonContinuar.SetActive(false);
        botonVolverMenu.SetActive(false);
        Application.Quit();
    }

    // Esperamos a que termine el audio del boton antes de salir del juego
    /*IEnumerator EsperarParaSalir(){
            
            audioSource.clip = aceptar;
            audioSource.Play();
    
            yield return new WaitForSeconds(3.0f);
    
            Debug.Log("SALIMOS DEL JUEGO EXITOSAMENTE");
            
    }*/
}
