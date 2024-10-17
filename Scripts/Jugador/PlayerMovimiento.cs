using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovimiento : MonoBehaviour
{

//              RECORDATORIOS
//  [SerializeField]                  = Se puede escribir su valor desde fuera como los publicos, pero no puede modificar desde otros lugares como los privados.
//  Animator.setBool(string, boolean) = Cambia el valor de un parámetro booleano.
//  Animator.setTrigger(string)       = Cambia el valor de un parámetro trigger.


    
    public static bool muerteJugador = false;

    public bool ataqueExitoso;
    public bool tocoSuelo = false;
    public GameObject cinemachine;

    [SerializeField] float velocidadEjeX;
    [SerializeField] float velocidadEjeY;
    [SerializeField] float rangoAtaque = 0.5f;
    [SerializeField] Transform hitBoxAtaque;
    [SerializeField] LayerMask enemigos;

    [HideInInspector] public Animator animaciones;

    private float tiempoEsperaEntreAtaques = 0f;
    private int jugadorAtaque;
    private bool corriendo = false;
    
    private Collider2D[] enemigosGolpeados;
    private SensorSuelo sensorSuelo;
    private Rigidbody2D movimientoXY;
    private LayerMask capaJugador;
    private LayerMask capaPlataforma;

    private float posicionActualX;
    private float posicionActualY;

    AtaqueController ataqueController;
    PlayerVida playerVida;
    CamaraSacudida camaraSacudida;

    SonidoEfecto jugadorSaltoSonido;
    SonidoEfecto jugadorAtaqueSonido;
    SonidoEfecto jugadorGameOverSonido;
    SonidoEfecto jugadorHeridaSonido;

    void Awake(){

        movimientoXY = GetComponent<Rigidbody2D>();
        animaciones = GetComponent<Animator>();
        sensorSuelo = transform.Find("SensorSuelo").GetComponent<SensorSuelo>(); 
        ataqueController = GameObject.Find("HeroKnight").GetComponent<AtaqueController>();
        playerVida = GameObject.Find("VidaExperiencia").GetComponent<PlayerVida>();
        camaraSacudida = GameObject.Find("MainCamera").GetComponent<CamaraSacudida>();

        capaJugador = LayerMask.NameToLayer("Jugador"); 
        capaPlataforma = LayerMask.NameToLayer("Plataforma"); 

        jugadorSaltoSonido = GameObject.Find("JugadorSalto").GetComponent<SonidoEfecto>();
        jugadorAtaqueSonido = GameObject.Find("JugadorAtaque").GetComponent<SonidoEfecto>();
        jugadorGameOverSonido = GameObject.Find("JugadorGameOver").GetComponent<SonidoEfecto>();
        jugadorHeridaSonido = GameObject.Find("JugadorHerida").GetComponent<SonidoEfecto>();

        // Activar camara que sigue al jugador
        cinemachine.SetActive(true);

    }

    void Start(){

        muerteJugador = false;
        animaciones.SetBool("Muerte", muerteJugador);
    }

    // Update is called once per frame
    void Update(){
        
        if(MenuPausaController.juegoPausado == false && muerteJugador == false){

            posicionActualX = movimientoXY.position.x;
            posicionActualY = movimientoXY.position.y;

            ///////////////////////////////////////////////////////////////
            //              COMPROBACION DE ESTADO DEL JUGADOR
            ///////////////////////////////////////////////////////////////

            // Comprobar si está o no en el suelo
            if ( !tocoSuelo && sensorSuelo.comprobarSuelo() ){
                tocoSuelo = true;
                animaciones.SetBool("TocoSuelo", tocoSuelo);
            } 

            else if ( tocoSuelo && !sensorSuelo.comprobarSuelo() ){
                tocoSuelo = false;
                animaciones.SetBool("TocoSuelo", tocoSuelo);
            }
            
            // Tiempo entre animaciones de ataque
            if(tiempoEsperaEntreAtaques > 0){
                tiempoEsperaEntreAtaques -= Time.deltaTime;
            }

            // Comprobar velocidad vertical para saber si esta subiendo o bajando
            animaciones.SetFloat("VelocidadEjeY", movimientoXY.velocity.y);


            ///////////////////////////////////////////////////////////////
            //                  MOVIMIENTO
            ///////////////////////////////////////////////////////////////

            // Atacar en tierra y aire
            if( Input.GetKeyDown(KeyCode.X) && tiempoEsperaEntreAtaques <= 0  && DialogoIndividual.estaHablando == false) {
                 
                if(tocoSuelo){
                    animaciones.SetTrigger("Ataque1");
                    StartCoroutine( HeroeAtaque(enemigosGolpeados) );
                }

                else{
                    animaciones.SetTrigger("Ataque2");
                    StartCoroutine( HeroeAtaque(enemigosGolpeados) );
                }

                tiempoEsperaEntreAtaques = 0.5f;
            }

            // Saltar
            if (Input.GetKeyDown(KeyCode.Z) && tocoSuelo && DialogoIndividual.estaHablando == false){
                
                jugadorSaltoSonido.Sonido();

                // Modificar velocidad vertical, sin modificar la horizontal
                movimientoXY.velocity = new Vector2(movimientoXY.velocity.x, velocidadEjeY);
                sensorSuelo.Deshabilitar(0.3f);

                tocoSuelo = false;
                animaciones.SetBool("TocoSuelo", tocoSuelo);

                animaciones.SetTrigger("Salto");
            }

            // Correr
            // Si se pulsa flecha derecha o izquierda para moverse horizontalmente
            // Se evita moverse en conversaciones.
            if( Input.GetKey(KeyCode.RightArrow) && DialogoIndividual.estaHablando == false){

                movimientoXY.velocity = new Vector2(velocidadEjeX, movimientoXY.velocity.y);
/*
                if ( && posicionActualX == movimientoXY.position.x && posicionActualY == movimientoXY.position.y){
                    
                }
  */              
                RotarPersonaje(false);
            

            } else if( Input.GetKey(KeyCode.LeftArrow)  && DialogoIndividual.estaHablando == false){

                movimientoXY.velocity = new Vector2(-velocidadEjeX, movimientoXY.velocity.y);
/*
                if (!tocoSuelo && posicionActualX == movimientoXY.position.x && posicionActualY == movimientoXY.position.y){
                    movimientoXY.velocity = new Vector2(0, -3);
                }
*/
                RotarPersonaje(true);
            
            } 
            
            // No se ha pulsado nada. Idle.
            else {

                movimientoXY.velocity = new Vector2(0, movimientoXY.velocity.y);

                corriendo = false;
                animaciones.SetBool("Corriendo", corriendo);

            
            }

            // Si es true, mira a la izquierda, si es false mira a la derecha
            void RotarPersonaje(bool rotar){

                corriendo = true;
                animaciones.SetBool("Corriendo", corriendo);

                this.transform.rotation = Quaternion.Euler(0f, rotar ? 180f : 0f, 0f);
            }



        }
    }

    // Se sincroniza el ataque con las animaciones
    // Se comprueba los enemigos en el area y los daña
    public IEnumerator HeroeAtaque(Collider2D[] enemigosGolpeados){

        yield return new WaitForSeconds(0.2f);

        enemigosGolpeados = Physics2D.OverlapCircleAll(hitBoxAtaque.position, rangoAtaque, enemigos);
        ataque(enemigosGolpeados);
    }


    // Calcular ataque del jugador a los enemigos
    void ataque(Collider2D[] enemigosGolpeados){

        if(enemigosGolpeados.Length > 0){
            ataqueExitoso = true;
            jugadorAtaqueSonido.Sonido();
        }

        foreach(Collider2D enemigo in enemigosGolpeados){

            jugadorAtaque = ataqueController.AtaqueCalculo();
            
            enemigo.GetComponent<Enemigo>().AtaqueRecibido(jugadorAtaque);
            
        }
    }

    // Se ejecuta si el jugador es herido
    public void Herido(){

        if(MenuPausaController.juegoPausado == false && muerteJugador == false){

            animaciones.SetTrigger("Herida");
            camaraSacudida.SacudirCamara();

            if(playerVida.GetVidaActual() <= 0){
                jugadorGameOverSonido.Sonido();
                Muerte();
            } else{
                jugadorHeridaSonido.Sonido();
            }
        }
    }



    // Si se choca con una pared, que se caiga al suelo en vez de quedarse suspendido
    void OnCollisionEnter2D(Collision2D other) {
        if( other.gameObject.tag == "Suelo" && !tocoSuelo && movimientoXY.velocity.y <=0){
            movimientoXY.velocity = new Vector2(0, -3);
        }
    }

    void OnCollisionStay2D(Collision2D other) {
        if( other.gameObject.tag == "Suelo" && !tocoSuelo && movimientoXY.velocity.y <=0){
            movimientoXY.velocity = new Vector2(0, -3);
        }
    }

    // El jugador ha perdido toda la vida. Se reinicia el nivel.
    public void Muerte(){

        muerteJugador = true;
        movimientoXY.velocity = new Vector2(0, 0);

        animaciones.SetBool("Muerte", muerteJugador);
        animaciones.SetTrigger("MuerteJugador");
        cinemachine.SetActive(false);

        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        Invoke("RecargarNivel", 2f);
    }

    // Reiniciamos el nivel tras morir
    void RecargarNivel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // LO USO PARA TESTEAR RANGO DE ATAQUE, PODER VERLO MIENTRAS JUEGO
    
    void OnDrawGizmosSelected() {
        if( hitBoxAtaque == null ){
        return;
        }
        Gizmos.DrawWireSphere(hitBoxAtaque.position, rangoAtaque);
    }
    

    IEnumerator BajarPlataforma() {

       tocoSuelo = false;
       animaciones.SetBool("TocoSuelo", tocoSuelo);

       Physics2D.IgnoreLayerCollision(capaJugador,capaPlataforma, true);
       yield return new WaitForSeconds (1.0f);
       Physics2D.IgnoreLayerCollision(capaJugador,capaPlataforma, false);
    }

}
