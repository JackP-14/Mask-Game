using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SequentialGlitchCycler : MonoBehaviour
{
    [Header("Componente Visual")]
    public Image imagenEnPantalla;

    // --- LAS TRES CLASES DE IMÁGENES NECESARIAS ---

    [Header("1. Las 5 Imágenes Estáticas (Pasos 1-5)")]
    [Tooltip("Pon aquí Size: 5. Son las fotos 'quietas' entre transiciones.")]
    public Sprite[] imagenesEstaticas; 

    [Header("2. La Transición Glitch (Intermedia)")]
    [Tooltip("La animación corta que suena con el grito entre cada paso.")]
    public Sprite[] secuenciaGlitchTransition;
    [Range(0.01f, 0.2f)]
    public float velocidadTransicion = 0.05f;

    [Header("3. La Secuencia Final (Paso 6 - Infinito)")]
    [Tooltip("El bucle eterno que queda al final.")]
    public Sprite[] secuenciaFinalLoop;
    [Range(0.01f, 0.5f)]
    public float velocidadFinalLoop = 0.1f;

    // ----------------------------------------------

    [Header("Audio")]
    public AudioClip[] gritos;
    private AudioSource audioSource;

    private int indiceActual = 0; // Para saber en qué imagen estática vamos (0-4)
    private bool enTransicion = false; // Para bloquear clicks mientras se anima
    private bool bucleFinalActivo = false; // Para saber si hemos llegado al final

    private int updated_lives = 10;
    void Start()
    {
        updated_lives = SaveData.Instance.lives;
        audioSource = GetComponent<AudioSource>();

        // Empezamos mostrando la primera imagen estática
        if (imagenesEstaticas.Length > 0)
        {
            imagenEnPantalla.sprite = imagenesEstaticas[0];
        }
    }

    void Update()
    {
        // Si hay click izquierdo Y NO estamos en mitad de una transición Y NO hemos llegado al final
        if (updated_lives != SaveData.Instance.lives)
        {
            updated_lives = SaveData.Instance.lives;
            if (!enTransicion && !bucleFinalActivo)
            {
                StartCoroutine(HacerTransicion());
            }
        }
    }

    // Esta corrutina maneja TODO el proceso de cambio
    IEnumerator HacerTransicion()
    {
        enTransicion = true; // Bloqueamos inputs

        // A) Reproducir Grito
        ReproducirGritoRandom();

        // B) Reproducir la animación de Glitch de transición
        foreach (Sprite frame in secuenciaGlitchTransition)
        {
            imagenEnPantalla.sprite = frame;
            yield return new WaitForSeconds(velocidadTransicion);
        }

        // C) Calcular el siguiente paso
        indiceActual++;

        // Si aún estamos dentro del rango de las 5 imágenes (índices 0 a 4)
        if (indiceActual < imagenesEstaticas.Length)
        {
            // Ponemos la siguiente imagen estática
            imagenEnPantalla.sprite = imagenesEstaticas[indiceActual];
            enTransicion = false; // Desbloqueamos para el siguiente click
        }
        else
        {
            // Si nos hemos pasado del índice 4, toca el final
            bucleFinalActivo = true; // Marcamos que es el final para no aceptar más clicks
            StartCoroutine(BucleFinalInfinito()); // Arrancamos el loop eterno
            // Nota: No ponemos 'enTransicion = false' porque ya no queremos más inputs.
        }
    }

    IEnumerator BucleFinalInfinito()
    {
        int frameLoop = 0;
        while (true)
        {
            imagenEnPantalla.sprite = secuenciaFinalLoop[frameLoop];
            frameLoop = (frameLoop + 1) % secuenciaFinalLoop.Length;
            yield return new WaitForSeconds(velocidadFinalLoop);
        }
    }

    void ReproducirGritoRandom()
    {
        if (gritos.Length > 0 && audioSource != null)
        {
            audioSource.PlayOneShot(gritos[Random.Range(0, gritos.Length)]);
        }
    }
}