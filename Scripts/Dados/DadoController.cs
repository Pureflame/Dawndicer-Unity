using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DadoController : MonoBehaviour
{

    [SerializeField] GameObject dadoActual;
    [SerializeField] GameObject dadoSecundario;

    [HideInInspector] public static bool dadoSecundarioLleno = true;

    public Sprite d4;
    public Sprite d6;
    public Sprite d8;
    public Sprite d10;
    public Sprite d12;
    public Sprite d20;

    private int usosDadoMaximo = 5;
    private int usosDadoActual;
    private Sprite daux;
    private TextMeshProUGUI usosDadoTexto;

    PlayerMovimiento playerMovimiento;
    SonidoEfecto cambiarDadoSonido;


    void Awake(){

        playerMovimiento = GameObject.Find("HeroKnight").GetComponent<PlayerMovimiento>();
        usosDadoTexto = GameObject.Find("TextoUsos").GetComponent<TextMeshProUGUI>();
        cambiarDadoSonido = GameObject.Find("DadoCambiar").GetComponent<SonidoEfecto>();
    }

    void Start(){

        usosDadoActual = usosDadoMaximo;
        usosDadoTexto.text = "" + usosDadoActual;
        dadoSecundarioLleno = true;
    }

    void Update()
    {
        cambiarDado();
        reducirUsosDado();
    }

    // Elige aleatoriamente el dado que soltara un enemigo muerto
    public Sprite DadosDropeo(){

        switch ( Random.Range(0,5) )
                {
                case 0:
                    daux = d6;
                    break;
                case 1:
                    daux = d8;
                    break;
                case 2:
                    daux = d10;
                    break;
                case 3:
                    daux = d12;
                    break;
                case 4:
                    daux = d20;
                    break;
                default:
                    daux = d6;
                    break;
                }

        return daux;
    }


    // Tras acertar un ataque, reducir la cantidad de usos que tiene ese dado
    void reducirUsosDado(){

        if(playerMovimiento.ataqueExitoso == true && dadoActual.GetComponent<Image>().sprite != d4){

                usosDadoActual--;

                if(usosDadoActual > 0){
                    usosDadoTexto.text = "" + usosDadoActual;
                }
                
               // Debug.Log("Dado dañado. Ahora tiene " + usosDadoActual + " restantes");
        }
        playerMovimiento.ataqueExitoso = false;
    }




    // Si tiene un dado secundario, se cambia el principal por el secundario.
    // Si no tiene un dado secundario, se comprueba si el dado es D4 u otro.
    // Si es otro, y el dado se desgasta, entonces se cambia por un D4 para que el jugador siempre tenga uno.
    void cambiarDado(){

        if( (Input.GetKeyDown(KeyCode.Space) && dadoSecundarioLleno == true && DialogoIndividual.estaHablando == false) || usosDadoActual <= 0 ){

            cambiarDadoSonido.Sonido();
            //StartCoroutine("CambiarDado");
            //Debug.Log(usosDadoActual + "    Dados cambiados");

            if(dadoActual.GetComponent<Image>().sprite != d4 && dadoSecundarioLleno == false){

                    usosDadoActual = usosDadoMaximo;
                    usosDadoTexto.text = "" + usosDadoActual;
                    dadoActual.GetComponent<Image>().sprite = d4;
                    //Debug.Log("CAMBIO FORZADO");
            }
            else{
                    usosDadoActual = usosDadoMaximo;
                    usosDadoTexto.text = "" + usosDadoActual;
                    dadoActual.GetComponent<Image>().sprite = dadoSecundario.GetComponent<Image>().sprite;
                    
                    //dadoSecundario.GetComponent<Image>().sprite = noDado;
                    dadoSecundario.gameObject.SetActive(false);
                    dadoSecundarioLleno = false;
                    //Debug.Log("CAMBIO NORMAL");
            }
        }

    }

    IEnumerator CambiarDado() {

       if(dadoActual.GetComponent<Image>().sprite != d4 && dadoSecundarioLleno == false){

            usosDadoActual = usosDadoMaximo;
            usosDadoTexto.text = "" + usosDadoActual;
            dadoActual.GetComponent<Image>().sprite = d4;
            yield return new WaitForSeconds (0.1f);
            //Debug.Log("CAMBIO FORZADO");
       }
       else{
            usosDadoActual = usosDadoMaximo;
            usosDadoTexto.text = "" + usosDadoActual;
            dadoActual.GetComponent<Image>().sprite = dadoSecundario.GetComponent<Image>().sprite;
            yield return new WaitForSeconds (0.1f);
            //dadoSecundario.GetComponent<Image>().sprite = noDado;
            dadoSecundario.gameObject.SetActive(false);
            dadoSecundarioLleno = false;
            //Debug.Log("CAMBIO NORMAL");
       }
    }
}
