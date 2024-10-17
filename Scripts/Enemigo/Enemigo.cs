using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour
{
    public DatosEnemigo datosEnemigo;
    public GameObject victoriaMensaje;

    [SerializeField] LayerMask jugador;
    [HideInInspector] public Animator animaciones;

    private bool corriendo = true;
    //private bool PlayerDetectado = false;
    private bool estaDentroEnemigo = false;
    private float cargaAtaque;
    private float esperaIzquierda;
    private float rangoAtaque;
    private float esperaDerecha;
    private int vidaActual;
    private Rigidbody2D movimientoXY;
    private Transform hitBoxAtaqueEnemigo;
    private TextMeshPro vida;
    private Collider2D[] jugadorGolpeado;

    private bool suelo = false;

    PlayerExperiencia playerExperiencia;
    DadoController dadoController;
    DetectarPlayer detectarPlayer;
    Sprite dadoDropeo;
    PlayerVida playerVida;
    SensorSueloDelanteEnemigo sensorSueloDelanteEnemigo;
    SonidoEfecto enemigoMuerteSonido;
    SonidoEfecto victoriaMusica;


    void Awake(){

        movimientoXY = GetComponent<Rigidbody2D>();
        animaciones = this.GetComponent<Animator>();
        detectarPlayer = GetComponentInChildren<DetectarPlayer>();
        dadoDropeo = GetComponentInChildren<SpriteRenderer>().sprite;
        sensorSueloDelanteEnemigo = GetComponentInChildren<SensorSueloDelanteEnemigo>();
        
        playerExperiencia = GameObject.Find("VidaExperiencia").GetComponent<PlayerExperiencia>();
        dadoController = GameObject.Find("DadoController").GetComponent<DadoController>();
        playerVida = GameObject.Find("VidaExperiencia").GetComponent<PlayerVida>();
        vida = this.gameObject.GetComponentInChildren<TextMeshPro>();

        enemigoMuerteSonido = GameObject.Find("EnemigoMuerte").GetComponent<SonidoEfecto>();
        victoriaMusica = GameObject.Find("MusicaVictoria").GetComponent<SonidoEfecto>();

        victoriaMensaje.SetActive(false);
    }


    
    void Start(){
/*
        if(vida == null){
            Debug.Log("ERRRRROR");
        }
        else{
        Debug.Log($"Noise Component: {vida}");
        }
*/
        vidaActual = datosEnemigo.vidaEnemigo;
        vida.text = "" + vidaActual;
        this.transform.GetChild(0).gameObject.SetActive(false);

        esperaDerecha = 0f;
        esperaIzquierda = datosEnemigo.tiempoEspera;

        rangoAtaque = datosEnemigo.rangoAtaque;
        hitBoxAtaqueEnemigo = datosEnemigo.hitBoxAtaqueEnemigo;
        cargaAtaque = datosEnemigo.tiempoCarga;

        estaDentroEnemigo = false;
        suelo = sensorSueloDelanteEnemigo.comprobarSuelo();  
    }


    void Update(){

        // Poner la vida siempre recta
        this.transform.GetChild(4).transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // Ignora a otros enemigos para que se atraviesen entre ellos.
        Physics2D.IgnoreLayerCollision(8,8,true);

        // Si el jugador esta dentro de este enemigo durante mucho tiempo recibira daño.
        HeridaConstante();

        if(detectarPlayer.detectadoPlayer == false){
            Rastreo();
        }

        if(detectarPlayer.detectadoPlayer == true){
            CargarAtaque();
        }
    }


    // El enemigo detecta al jugador y comienza a cargar su ataque.
    public void CargarAtaque(){

        corriendo = false;
        animaciones.SetBool("Corriendo", corriendo);

        movimientoXY.velocity = new Vector2(0, 0);

        if(cargaAtaque >=0){

            cargaAtaque -= Time.deltaTime;
        } else{
            AtacarJugador();
        }
    }


    // El enemigo ataca al jugador tras terminar de cargar el ataque.
    // Vuelve a comenzar a cargar su ataque.
    public void AtacarJugador(){

        animaciones.SetTrigger("Ataque");

        if(datosEnemigo.nombreEnemigo == "Goblin"){
            StartCoroutine( GoblinAtaque(jugadorGolpeado) );
        } else if(datosEnemigo.nombreEnemigo == "Murcielago"){
            StartCoroutine( MurcielagoAtaque(jugadorGolpeado) );
        } else {
            StartCoroutine( GolemAtaque(jugadorGolpeado) );
        }

        cargaAtaque = datosEnemigo.tiempoCarga;
    }

    // Se sincroniza el ataque con las animaciones
    public IEnumerator MurcielagoAtaque(Collider2D[] jugadorGolpeado){

        yield return new WaitForSeconds(0.25f);

        jugadorGolpeado = Physics2D.OverlapCircleAll(hitBoxAtaqueEnemigo.position, rangoAtaque, jugador);

        foreach(Collider2D jugador in jugadorGolpeado){
            playerVida.AumentarReducirVidaActual(-datosEnemigo.ataqueEnemigo);
        }
    }

    // Se sincroniza el ataque con las animaciones
    public IEnumerator GoblinAtaque(Collider2D[] jugadorGolpeado){

        yield return new WaitForSeconds(0.2f);

        jugadorGolpeado = Physics2D.OverlapCircleAll(hitBoxAtaqueEnemigo.position, rangoAtaque, jugador);

        foreach(Collider2D jugador in jugadorGolpeado){
            playerVida.AumentarReducirVidaActual(-datosEnemigo.ataqueEnemigo);
        }
    }

    // Se sincroniza el ataque con las animaciones
    public IEnumerator GolemAtaque(Collider2D[] jugadorGolpeado){

        yield return new WaitForSeconds(0.5f);

        jugadorGolpeado = Physics2D.OverlapCircleAll(hitBoxAtaqueEnemigo.position, rangoAtaque, jugador);

        foreach(Collider2D jugador in jugadorGolpeado){
            playerVida.AumentarReducirVidaActual(-datosEnemigo.ataqueEnemigo);
        }
    }


    // El enemigo busca al jugador moviendose de izquierda a derecha
    public void Rastreo(){

        corriendo = true;
        animaciones.SetBool("Corriendo", corriendo);

        if(suelo == true && sensorSueloDelanteEnemigo.comprobarSuelo() == false){
            CambiarSentidoForzado();
        }
        suelo = sensorSueloDelanteEnemigo.comprobarSuelo();

        // Se mueve a la derecha
        if(esperaIzquierda > 0f){
            esperaIzquierda -= Time.deltaTime;

            if(esperaIzquierda <= 0){
                esperaDerecha = datosEnemigo.tiempoEspera;
            }

        } else {
            
            EnemigoMueveDerecha();
        }

        // Se mueve a la izquierda
        if(esperaDerecha > 0f){
            esperaDerecha -= Time.deltaTime;

            if(esperaDerecha <= 0){
                esperaIzquierda = datosEnemigo.tiempoEspera;
            }

        } else {
        
            EnemigoMueveIzquierda();
        }
    }

    public void CambiarSentidoForzado(){

        if(esperaIzquierda > 0f){

            esperaIzquierda = 0f;
            esperaDerecha = datosEnemigo.tiempoEspera;
            EnemigoMueveIzquierda();

        } else{

            esperaDerecha = 0f;
            esperaIzquierda = datosEnemigo.tiempoEspera;
            EnemigoMueveDerecha();
        }

    }

    public void EnemigoMueveDerecha(){

        if(MenuPausaController.juegoPausado == true){

            movimientoXY.velocity = new Vector2(0, 0);
            RotarEnemigo(false);
        } else {
            movimientoXY.velocity = new Vector2(datosEnemigo.velocidadMovimiento, 0);
            RotarEnemigo(false);
        }
    }

    public void EnemigoMueveIzquierda(){

        if(MenuPausaController.juegoPausado == true){

            movimientoXY.velocity = new Vector2(0, 0);
            RotarEnemigo(true);
        } else {
            movimientoXY.velocity = new Vector2(-datosEnemigo.velocidadMovimiento, 0);
            RotarEnemigo(true);
        }
    }


    // Si es true, mira a la izquierda, si es false mira a la derecha.
    void RotarEnemigo(bool rotar){

        this.transform.rotation = Quaternion.Euler(0f, rotar ? 180f : 0f, 0f);
    }


    // El jugador ha tocado al enemigo y recibe daño por ello.
    void OnTriggerEnter2D(Collider2D other) {
        
        if( other.gameObject.tag == "Player" && PlayerVida.tiempoInvencibilidad <= 0 && PlayerMovimiento.muerteJugador == false){
            estaDentroEnemigo = true;
            int ataque = datosEnemigo.ataqueEnemigo;

                playerVida.AumentarReducirVidaActual(-ataque);
            //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }


    // El jugador ya no toca al enemigo.
    // Por tanto, ya no recibe daño constante.
    void OnTriggerExit2D(Collider2D other) {
       
        if( other.gameObject.tag == "Player"){
            estaDentroEnemigo = false;
        }
        
    }


    // Si el jugador sigue dentro de un enemigo cuando se le acaba la invencibilidad,
    // recibira daño nuevamente.
    public void HeridaConstante(){
        if(estaDentroEnemigo == true && PlayerVida.tiempoInvencibilidad <= 0 && PlayerMovimiento.muerteJugador == false){

            int ataque = datosEnemigo.ataqueEnemigo;

            playerVida.AumentarReducirVidaActual(-ataque);
        }
    }


    // El enemigo recibe un ataque del jugador.
    // Si el enemigo estaba cargando un ataque, vuelve a empezar a cargarlo.
    public void AtaqueRecibido(int ataque){

        animaciones.SetTrigger("Hit");
        vidaActual -= ataque;
        vida.text = "" + vidaActual;
        
        if(datosEnemigo.nombreEnemigo != "Golem"){
            cargaAtaque = datosEnemigo.tiempoCarga;
        }

        

        if(vidaActual <= 0){

            Muerte();
        }
    }


    // El enemigo muere y suelta un dado.
    public void Muerte(){

        animaciones.SetBool("Muerto", true);
        enemigoMuerteSonido.Sonido();

        if(datosEnemigo.nombreEnemigo == "Murcielago"){
            playerExperiencia.AumentarExperienciaActual(1);
            this.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        if(datosEnemigo.nombreEnemigo == "Goblin"){
            playerExperiencia.AumentarExperienciaActual(2);
        }

        if(datosEnemigo.nombreEnemigo == "Golem"){
            //playerExperiencia.AumentarExperienciaActual(5);
            victoriaMensaje.SetActive(true);
            victoriaMusica.Sonido();
            Invoke("FinDePartida", 3f);
        }

        // Desaparece el indicador de la vida
        this.transform.GetChild(4).gameObject.SetActive(false);

        if(datosEnemigo.nombreEnemigo == "Golem"){
        } else {

            // Ponemos el dado dropeado aleatoriamente
            dadoDropeo = dadoController.DadosDropeo();
            this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = dadoDropeo;

            // Poner el dado dropeado siempre en la misma posicion, sin mirar a la izquierda
            this.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        
    }

    void FinDePartida(){
        SceneManager.LoadScene(0);
    }


    void OnDrawGizmosSelected() {
        if( hitBoxAtaqueEnemigo == null ){
        return;
        }
        Gizmos.DrawWireSphere(hitBoxAtaqueEnemigo.position, rangoAtaque);
    }

}


// Datos del enemigo a utilizar.
// Podemos crear varios enemigos de esta forma.
[System.Serializable] public class DatosEnemigo{

    public string nombreEnemigo;

    public int ataqueEnemigo;
    public int vidaEnemigo;
    public int velocidadMovimiento;

    public float tiempoEspera;
    public float tiempoCarga;
    public float rangoAtaque;
    public Transform hitBoxAtaqueEnemigo;

}

