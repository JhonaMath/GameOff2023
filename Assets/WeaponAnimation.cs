using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
     public float amplitud = 1.0f; // Amplitud de la oscilación (cuánto se mueve a cada lado)
    public float velocidad = 1.0f; // Velocidad de oscilación (cuán rápido oscila)
    private Vector3 posicionInicial;

    

    void Start()
    {
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        // Calcula la nueva posición en función del tiempo
        float offsetY = Mathf.Sin(Time.time * velocidad) * amplitud;
        // Vector3 nuevaPosicion = posicionInicial + new Vector3(0, offsetY, 0);

        // Aplica la nueva posición al objeto
        transform.localPosition = new Vector3(transform.localPosition.x, offsetY + posicionInicial.y, 0);
    }
}
