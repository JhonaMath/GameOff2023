using UnityEngine;

public class GirarHaciaMouse : MonoBehaviour
{

    public Transform transformObj;

    void Update()
    {

        // Obtén la posición del mouse en el mundo
        // Vector3 posicionMouseEnMundo = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        // Debug.Log(Input.mousePosition);
    
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        // transformObj.position = mousePos;

        // Calcula la dirección hacia la posición del mouse
        Vector3 direccion = mousePos - transform.position;

        // Calcula el ángulo entre la dirección y la orientación actual del objeto
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // Gira el objeto hacia la posición del mouse
         // Gira el objeto hacia la posición del mouse
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo));
    }
    
}