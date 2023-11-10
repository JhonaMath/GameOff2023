using UnityEngine;

public class FieldOfView2D : MonoBehaviour
{
    public float radioMaximo = 5.0f; // Radio máximo del campo de visión.
    public float anguloMaximo = 60.0f; // Ángulo máximo del campo de visión.
    public LayerMask capasVisibles; // Capas que pueden ser visibles.

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 3;
    }

    void Update()
    {
        // Obtener la posición del mouse en la pantalla
        Vector3 posicionMouse = Input.mousePosition;
        posicionMouse.z = -Camera.main.transform.position.z; // Ajustar la posición del mouse en Z

        // Convertir la posición del mouse de píxeles a coordenadas del mundo
        Vector3 posicionMouseEnMundo = Camera.main.ScreenToWorldPoint(posicionMouse);

        // Calcular la dirección hacia la posición del mouse
        Vector3 direccionAlMouse = posicionMouseEnMundo - transform.position;

        // Ajustar el radio y el ángulo del FOV según la distancia al mouse
        float radioActual = Mathf.Min(radioMaximo, direccionAlMouse.magnitude);
        float anguloActual = Mathf.Min(anguloMaximo, Mathf.Atan2(direccionAlMouse.y, direccionAlMouse.x) * Mathf.Rad2Deg * 2);

        // Actualizar la representación visual del FOV
        UpdateFOVRenderer(radioActual, anguloActual);

        // Detectar objetos dentro del FOV
        DetectarObjetosEnFOV();
    }

    void UpdateFOVRenderer(float radioActual, float anguloActual)
    {
        // Actualizar la línea del FOV basado en el radio y el ángulo
        float x = Mathf.Cos(anguloActual * 0.5f * Mathf.Deg2Rad) * radioActual;
        float y = Mathf.Sin(anguloActual * 0.5f * Mathf.Deg2Rad) * radioActual;

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, new Vector3(x, y, 0));
        lineRenderer.SetPosition(2, new Vector3(x, -y, 0));
    }

    void DetectarObjetosEnFOV()
    {
        Collider2D[] objetosEnFOV = Physics2D.OverlapCircleAll(transform.position, radioMaximo, capasVisibles);

        foreach (Collider2D objeto in objetosEnFOV)
        {
            // Realiza acciones con los objetos dentro del FOV según tus necesidades
            Debug.Log("Objeto visible: " + objeto.name);
        }
    }
}