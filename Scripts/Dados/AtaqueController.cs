using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtaqueController : MonoBehaviour
{

    public static int extraMenor;
    public static int extraMayor;

    [SerializeField] GameObject dadoActual;
    
    private int ataque;
    private int rangoAtaqueMenor;
    private int rangoAtaqueMayor;
    private Sprite dadoAtaque;
    private TextMeshProUGUI resultadoDados;

    DadoController dadoController;

    private void Awake() {

        dadoController = GameObject.Find("DadoController").GetComponent<DadoController>();
        resultadoDados = GameObject.Find("Resultado").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        extraMayor = 0;
        extraMenor = 0;
        resultadoDados.text = "0";
    }

    public int AtaqueCalculo(){

        // Comprobamos el dado actual para elegir el tipo de daño
        dadoAtaque = dadoActual.GetComponent<Image>().sprite;

        ElegirAtaque(dadoAtaque);

        ataque = Random.Range(rangoAtaqueMenor, rangoAtaqueMayor);

        //Debug.Log(ataque);
        resultadoDados.text = "" + ataque;

        return ataque;

    }

    // Se escoge el intervalo de ataque segun el dado.
    // Tambien se aumenta el intervalo segun el nivel del personaje
    public void ElegirAtaque(Sprite dadoAtaque){
        
        if( dadoAtaque.Equals(dadoController.d4) ){

            rangoAtaqueMenor = 1 + extraMenor;
            rangoAtaqueMayor = 5 + extraMayor;

        } else if( dadoAtaque.Equals(dadoController.d6) ) {

            rangoAtaqueMenor = 1 + extraMenor;
            rangoAtaqueMayor = 7 + extraMayor;

        } else if( dadoAtaque.Equals(dadoController.d8) ) {

            rangoAtaqueMenor = 1 + extraMenor;
            rangoAtaqueMayor = 9 + extraMayor;

        } else if( dadoAtaque.Equals(dadoController.d10) ) {

            rangoAtaqueMenor = 1 + extraMenor;
            rangoAtaqueMayor = 11 + extraMayor;

        } else if( dadoAtaque.Equals(dadoController.d12) ) {

            rangoAtaqueMenor = 1 + extraMenor;
            rangoAtaqueMayor = 13 + extraMayor;

        } else if( dadoAtaque.Equals(dadoController.d20) ) {

            rangoAtaqueMenor = 1 + extraMenor;
            rangoAtaqueMayor = 21 + extraMayor;

        } else {

            rangoAtaqueMenor = 1;
            rangoAtaqueMayor = 2 + extraMayor;

        }
    }



}
