using UnityEngine;
using UnityEngine.InputSystem;

public class Zoom2CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform Camera;

    [Header("Sensibilidad de Movimiento")]
    [SerializeField] private float sensibilidad = 5f;

    [Header("Zona de Activación (% de pantalla)")]
    [Tooltip("Porcentaje de la pantalla desde el borde donde se activa el movimiento (0-1)")]
    [SerializeField] private float zonaActivacion = 0.15f;

    [Header("Límites de Movimiento de la Cámara")]
    [SerializeField] private float limiteIzquierdo = -10f;
    [SerializeField] private float limiteDerecho = 10f;
    [SerializeField] private float limiteInferior = -10f;
    [SerializeField] private float limiteSuperior = 10f;

    [Header("Suavizado (opcional)")]
    [SerializeField] private bool usarSuavizado = true;
    [SerializeField] private float velocidadSuavizado = 5f;

    private Vector3 posicionObjetivo;

    void OnEnable()
    {
        posicionObjetivo = Camera.position;
    }

    void Update()
    {
        // Obtener la posición del ratón usando el NEW INPUT SYSTEM
        Vector2 mousePosPixels = Mouse.current.position.ReadValue();

        // Convertir a coordenadas normalizadas (0-1)
        Vector2 mousePos = new Vector2(
            mousePosPixels.x / Screen.width,
            mousePosPixels.y / Screen.height
        );

        // Calcular el movimiento
        Vector3 movimiento = Vector3.zero;

        // Movimiento horizontal
        if (mousePos.x <= zonaActivacion) // Izquierda
        {
            float intensidad = 1 - (mousePos.x / zonaActivacion);
            movimiento.x = -intensidad * sensibilidad * Time.deltaTime;
        }
        else if (mousePos.x >= 1 - zonaActivacion) // Derecha
        {
            float intensidad = (mousePos.x - (1 - zonaActivacion)) / zonaActivacion;
            movimiento.x = intensidad * sensibilidad * Time.deltaTime;
        }

        // Movimiento vertical
        if (mousePos.y <= zonaActivacion) // Abajo
        {
            float intensidad = 1 - (mousePos.y / zonaActivacion);
            movimiento.y = -intensidad * sensibilidad * Time.deltaTime;
        }
        else if (mousePos.y >= 1 - zonaActivacion) // Arriba
        {
            float intensidad = (mousePos.y - (1 - zonaActivacion)) / zonaActivacion;
            movimiento.y = intensidad * sensibilidad * Time.deltaTime;
        }

        // Aplicar el movimiento a la posición objetivo
        posicionObjetivo += movimiento;

        // Aplicar límites
        posicionObjetivo.x = Mathf.Clamp(posicionObjetivo.x, limiteIzquierdo, limiteDerecho);
        posicionObjetivo.y = Mathf.Clamp(posicionObjetivo.y, limiteInferior, limiteSuperior);
        posicionObjetivo.z = Camera.position.z;

        // Mover la cámara
        if (usarSuavizado)
        {
            Camera.position = Vector3.Lerp(Camera.position, posicionObjetivo, velocidadSuavizado * Time.deltaTime);
        }
        else
        {
            Camera.position = posicionObjetivo;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Camera == null) return;

        Gizmos.color = Color.yellow;

        // Dibujar límites
        Vector3 esquinaInfIzq = new Vector3(limiteIzquierdo, limiteInferior, Camera.position.z);
        Vector3 esquinaInfDer = new Vector3(limiteDerecho, limiteInferior, Camera.position.z);
        Vector3 esquinaSupDer = new Vector3(limiteDerecho, limiteSuperior, Camera.position.z);
        Vector3 esquinaSupIzq = new Vector3(limiteIzquierdo, limiteSuperior, Camera.position.z);

        Gizmos.DrawLine(esquinaInfIzq, esquinaInfDer);
        Gizmos.DrawLine(esquinaInfDer, esquinaSupDer);
        Gizmos.DrawLine(esquinaSupDer, esquinaSupIzq);
        Gizmos.DrawLine(esquinaSupIzq, esquinaInfIzq);

        }
    }