using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogoController : MonoBehaviour
{

    // "Queue<>" es como un array pero FIFO (First In, First Out)
    private Queue<string> queueFrasesDialogo;
    private TextMeshProUGUI nombreDialogoUI;
    private TextMeshProUGUI textoDialogoUI;


    void Awake() {
        
        nombreDialogoUI = GameObject.Find("NombreDialogo").GetComponent<TextMeshProUGUI>();
        textoDialogoUI = GameObject.Find("TextoDialogo").GetComponent<TextMeshProUGUI>();
    }

    void Start(){

        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        queueFrasesDialogo = new Queue<string>();
    }

    // Inicio del dialogo.
    public void IniciarDialogo(DialogoDatos dialogo){

        // Se activa la interfaz de dialogo
        gameObject.transform.GetChild(1).gameObject.SetActive(true);

        nombreDialogoUI.text = dialogo.nombrePersonaje;

        // Limpiamos la cola de una conversacion anterior
        queueFrasesDialogo.Clear();

        foreach (string frase in dialogo.frasesDialogo){

            // Insertamos la frase en la cola de frases
            queueFrasesDialogo.Enqueue(frase); 
        }

        SiguienteFrase();

    }

    

    // Se continua la conversacion si quedan elementos en la cola.
    // Sino, se termina.
    public void SiguienteFrase(){

        // En vez de "lenght", Queue usa "Count" para medir su extension.
        if(queueFrasesDialogo.Count == 0){
            
            TerminarDialogo();

        } else {

            // Elimina el primer elemento de la cola y lo mete en el string.
            string fraseActual = queueFrasesDialogo.Dequeue();
            textoDialogoUI.text = fraseActual;
        }
    }

    public void TerminarDialogo(){

        // Se desactiva la interfaz de dialogo
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(DelayVariable());
        
    }

    IEnumerator DelayVariable() {

       yield return new WaitForSeconds (0.1f);
       DialogoIndividual.estaHablando = false;
    }

}
