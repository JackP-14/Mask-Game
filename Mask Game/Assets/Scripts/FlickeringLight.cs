using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Simula el parpadeo realista de antorchas o velas para Light 2D en Unity URP
/// </summary>
[RequireComponent(typeof(Light2D))]
public class FlickeringLight2D : MonoBehaviour
{
    [Header("Configuración de Intensidad")]
    [SerializeField] private float intensidadBase = 35f;
    [SerializeField] private float rangoParpadeo = 5f;
    [SerializeField] private float velocidadParpadeo = 5f;

    [Header("Configuración de Color")]
    [SerializeField] private bool usarVariacionColor = true;
    [SerializeField] private Color colorBase = new Color(1f, 0.6f, 0.2f); // Color naranja cálido
    [SerializeField] private float variacionColor = 0.1f;

    [Header("Configuración de Alcance (Outer Radius)")]
    [SerializeField] private bool usarVariacionAlcance = true;
    [SerializeField] private float radiusBase = 2.79f;
    [SerializeField] private float variacionRadius = 0.3f;

    [Header("Efectos Adicionales")]
    [SerializeField] private bool efectoRafagaViento = false;
    [SerializeField] private float probabilidadRafaga = 0.02f;
    [SerializeField] private float intensidadRafaga = 0.6f;

    private Light2D luzComponente;
    private float tiempoParpadeo;
    private float siguienteRafaga;
    private bool enRafaga = false;
    private float tiempoRafaga;

    void Start()
    {
        // Obtener el componente Light2D
        luzComponente = GetComponent<Light2D>();

        // Configurar valores iniciales
        luzComponente.intensity = intensidadBase;
        luzComponente.color = colorBase;

        // Si tiene outer radius (para Spot o Point lights)
        if (luzComponente.lightType == Light2D.LightType.Point)
        {
            radiusBase = luzComponente.pointLightOuterRadius;
        }

        // Inicializar tiempo aleatorio para variación
        tiempoParpadeo = Random.Range(0f, 100f);
        siguienteRafaga = Time.time + Random.Range(5f, 15f);
    }

    void Update()
    {
        // Incrementar el tiempo de parpadeo
        tiempoParpadeo += Time.deltaTime * velocidadParpadeo;

        // Efecto de ráfaga de viento ocasional
        if (efectoRafagaViento && !enRafaga && Time.time >= siguienteRafaga)
        {
            if (Random.value < probabilidadRafaga)
            {
                IniciarRafaga();
            }
        }

        // Calcular modificadores
        float modificadorRafaga = enRafaga ? CalcularEfectoRafaga() : 1f;

        // Aplicar parpadeo a la intensidad usando Perlin Noise para suavidad
        float parpadeoIntensidad = Mathf.PerlinNoise(tiempoParpadeo, 0f);
        float intensidadObjetivo = intensidadBase + (parpadeoIntensidad - 0.5f) * 2f * rangoParpadeo;
        intensidadObjetivo *= modificadorRafaga;

        luzComponente.intensity = Mathf.Lerp(luzComponente.intensity, intensidadObjetivo, Time.deltaTime * 10f);

        // Variar el color ligeramente
        if (usarVariacionColor)
        {
            float parpadeoColor = Mathf.PerlinNoise(tiempoParpadeo * 0.5f, 100f);
            Color colorObjetivo = colorBase + new Color(
                (parpadeoColor - 0.5f) * variacionColor,
                (parpadeoColor - 0.5f) * variacionColor * 0.5f,
                0f
            );
            luzComponente.color = Color.Lerp(luzComponente.color, colorObjetivo, Time.deltaTime * 5f);
        }

        // Variar el alcance (outer radius)
        if (usarVariacionAlcance && (luzComponente.lightType == Light2D.LightType.Point))
        {
            float parpadeoRadius = Mathf.PerlinNoise(tiempoParpadeo * 0.3f, 200f);
            float radiusObjetivo = radiusBase + (parpadeoRadius - 0.5f) * 2f * variacionRadius;
            radiusObjetivo *= modificadorRafaga;

            luzComponente.pointLightOuterRadius = Mathf.Lerp(
                luzComponente.pointLightOuterRadius,
                radiusObjetivo,
                Time.deltaTime * 8f
            );
        }
    }

    private void IniciarRafaga()
    {
        enRafaga = true;
        tiempoRafaga = 0f;
        siguienteRafaga = Time.time + Random.Range(10f, 30f);
    }

    private float CalcularEfectoRafaga()
    {
        tiempoRafaga += Time.deltaTime;

        // Duración de la ráfaga
        float duracionRafaga = Random.Range(0.3f, 0.8f);

        if (tiempoRafaga >= duracionRafaga)
        {
            enRafaga = false;
            return 1f;
        }

        // Curva de intensidad de la ráfaga
        float progreso = tiempoRafaga / duracionRafaga;
        float curva = Mathf.Sin(progreso * Mathf.PI);

        return 1f - (curva * intensidadRafaga);
    }

    /// <summary>
    /// Ajusta la intensidad base de la luz en tiempo real
    /// </summary>
    public void SetIntensidadBase(float nuevaIntensidad)
    {
        intensidadBase = nuevaIntensidad;
    }

    /// <summary>
    /// Cambia el color base de la luz
    /// </summary>
    public void SetColorBase(Color nuevoColor)
    {
        colorBase = nuevoColor;
    }

    /// <summary>
    /// Ajusta el radio base de la luz
    /// </summary>
    public void SetRadiusBase(float nuevoRadius)
    {
        radiusBase = nuevoRadius;
    }
}