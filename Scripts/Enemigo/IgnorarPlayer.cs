using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorarPlayer : MonoBehaviour
{

    private void Update() {
        //Sensor ignora al jugador, otros enemigos y sensores
        Physics2D.IgnoreLayerCollision(3,12,true);
        Physics2D.IgnoreLayerCollision(8,12,true);
        Physics2D.IgnoreLayerCollision(12,12,true);
    }
    
}
