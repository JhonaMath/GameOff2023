using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    
    [SerializeField] private FieldOfView fieldOfView;

    public float velocidad = 5.0f; // Velocidad de movimiento ajustable
    
    void Update()
    {
        // Verificar la entrada del jugador 
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Calcular la dirección de movimiento
        Vector3 movimiento = new Vector3(movimientoHorizontal, movimientoVertical, 0.0f);

        // Aplicar el movimiento al objeto
        transform.Translate(movimiento * velocidad * Time.deltaTime);
    
        Vector3 posicionMouse = Input.mousePosition;
        Vector3 aimDir = (posicionMouse);


    }
}
