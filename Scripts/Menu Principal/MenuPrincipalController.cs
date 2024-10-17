using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuPrincipalController : MonoBehaviour {

    [SerializeField] AudioClip aceptar;
    [SerializeField] AudioClip moverFlecha;
    [SerializeField] AudioClip musicaFondoMenu;
    [SerializeField] GameObject botonSalir;
    [SerializeField] GameObject botonControles;
    [SerializeField] GameObject botonJugar;
    [SerializeField] GameObject botonAtras;
    [SerializeField] GameObject textosControles;
    [SerializeField] GameObject tituloDawndicer;
    [SerializeField] GameObject flecha;

    private int botonSeleccionado;
    private GameObject seleccionado;
    
    
    AudioSource audioAceptar;
    AudioSource audioMenu;
    SonidoEfecto audioMover;

    void Awake() {
        audioAceptar = GetComponent<AudioSource>();
        audioMenu = GameObject.Find("Musica").GetComponent<AudioSource>();
        audioMover = GameObject.Find("MoverFlecha").GetComponent<SonidoEfecto>();

        botonAtras.SetActive(false);
        textosControles.SetActive(false);

        audioMenu.clip = musicaFondoMenu;
    }

    void Start()
    {

       
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonJugar);

        seleccionado = EventSystem.current.currentSelectedGameObject;
        seleccionarBoton();

        
        audioAceptar.clip = aceptar;
        audioMenu.Play();
        
    }

    // Mover la flecha junto al objeto seleccionado actualmente
    void Update() {


        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {

        }

        if(seleccionado != EventSystem.current.currentSelectedGameObject){
            audioMover.Sonido();
        }
        Debug.Log(seleccionado);
/*
        if( Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)){
            audioMover.Sonido();
        }
*/
        if(EventSystem.current.currentSelectedGameObject == null){
            EventSystem.current.SetSelectedGameObject(seleccionado);
        }
        seleccionado = EventSystem.current.currentSelectedGameObject;
        
        // Absorber clicks del raton ajenos a los botones
        if(seleccionado != botonJugar && seleccionado != botonControles && seleccionado != botonSalir && seleccionado != botonAtras){
            
        } else {
            flecha.transform.position = new Vector2(seleccionado.transform.position.x - 320, seleccionado.transform.position.y - 15);
            seleccionarBoton();
        }
        
        if(Input.GetKeyDown(KeyCode.Z)){

            switch (botonSeleccionado){

                // Jugar
                case 0:
                    //Debug.Log("Iniciar Partida");
                    IniciarPartida();
                    break;

                // Controles    
                case 1:
                    //Debug.Log("Controles");
                    MenuControlesEntrar();
                    break;

                // Salir
                case 2:
                    //Debug.Log("Salimos de los controles");
                    SalirJuego();
                    break;

                // Volver al menu principal
                case 3:
                    //Debug.Log("Salimos");
                    MenuControlesSalir();
                    break;

                default:
                    //Debug.Log("Hay un error en el menu principal");
                    break;
            }

        }


    }

    void retardo(){
        
    }

    public void seleccionarBoton(){
        
        if(seleccionado == botonJugar){
            botonSeleccionado = 0;
        }
        if(seleccionado == botonControles){
            botonSeleccionado = 1;
        }
        if(seleccionado == botonSalir){
            botonSeleccionado = 2;
        }
        if(seleccionado == botonAtras){
            botonSeleccionado = 3;
        }

    }

    public void MenuControlesEntrar(){

        botonJugar.SetActive(false);
        botonControles.SetActive(false);
        botonSalir.SetActive(false);
        tituloDawndicer.SetActive(false);

        botonAtras.SetActive(true);
        textosControles.SetActive(true);
        
        //audioAceptar.Play();

        //EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botonAtras);
    }

    public void MenuControlesSalir(){

        botonAtras.SetActive(false);
        textosControles.SetActive(false);

        botonJugar.SetActive(true);
        botonControles.SetActive(true);
        botonSalir.SetActive(true);
        tituloDawndicer.SetActive(true);

        //audioAceptar.Play();

        EventSystem.current.SetSelectedGameObject(botonControles);
    }

    // Iniciamos el primer nivel
    public void IniciarPartida(){
        
        //StartCoroutine(EsperarParaIniciar());
        botonControles.SetActive(false);
        botonSalir.SetActive(false);

        audioAceptar.Play();
        Invoke("IniciarNivel",0.5f);
    }

    void IniciarNivel(){
        SceneManager.LoadScene(1);
    }


    // Salimos del videojuego
    public void SalirJuego(){

        //StartCoroutine(EsperarParaSalir());
        botonJugar.SetActive(false);
        botonControles.SetActive(false);

        audioAceptar.Play();
        Invoke("ApagarJuego",1f);
    }

    void ApagarJuego(){
        //Debug.Log("SALIMOS DEL JUEGO EXITOSAMENTE");
        Application.Quit();
    }


    // Esperamos a que termine el audio del boton antes de iniciar el juego
/*
    IEnumerator EsperarParaIniciar(){
            
            audioSource.clip = aceptar;
            audioSource.Play();
    
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene(1);
            
    }
*/  
    


    // Esperamos a que termine el audio del boton antes de salir del juego
/*
    IEnumerator EsperarParaSalir(){
            
            audioSource.clip = aceptar;
            audioSource.Play();
    
            yield return new WaitForSeconds(3.0f);
    
            Debug.Log("SALIMOS DEL JUEGO EXITOSAMENTE");
            Application.Quit();
    }
*/




}