using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // Seguimos con el Input System nuevo
using System.Collections;

[RequireComponent(typeof(AudioSource))] // Esto añade el componente de audio automáticamente si no lo tienes
public class ImageCyclerCompleto : MonoBehaviour
{
    [Header("Visuales")]
    public Image imagenEnPantalla;
    public Sprite[] formas;         // Triángulo, Círculo, Cuadrado
    public Sprite[] framesDelEfecto; // Tu GIF descompuesto
    public float velocidadDelGif = 0.05f;

    [Header("Audio (Gritos)")]
    public AudioSource audioSource; // Arrastra el componente aquí
    public AudioClip[] gritos;      // Arrastra tus archivos de audio aquí

    private int indexForma = 0;
    private bool isAnimating = false;

    void Start()
    {
        // Si no asignaste el AudioSource manual, lo buscamos
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (formas.Length > 0)
        {
            imagenEnPantalla.sprite = formas[0];
        }
    }

    void Update()
    {
        // Input System: Click izquierdo
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isAnimating)
            {
                StartCoroutine(SecuenciaCambio());
            }
        }
    }

    IEnumerator SecuenciaCambio()
    {
        isAnimating = true;

        // 1. REPRODUCIR GRITO ALEATORIO
        ReproducirGritoRandom();

        // 2. REPRODUCIR EFECTO VISUAL (GIF)
        foreach (Sprite frame in framesDelEfecto)
        {
            imagenEnPantalla.sprite = frame;
            yield return new WaitForSeconds(velocidadDelGif);
        }

        // 3. CAMBIAR A LA SIGUIENTE FORMA
        indexForma++;
        if (indexForma >= formas.Length)
        {
            indexForma = 0; // Vuelta al principio
        }
        imagenEnPantalla.sprite = formas[indexForma];

        isAnimating = false;
    }

    void ReproducirGritoRandom()
    {
        // Seguridad: Solo suena si hay clips y un AudioSource
        if (gritos.Length > 0 && audioSource != null)
        {
            // Elegimos un número al azar entre 0 y el total de gritos
            int indexAleatorio = Random.Range(0, gritos.Length);
            
            // Usamos PlayOneShot para que puedan solaparse si fuera necesario y variar el volumen si quieres
            audioSource.PlayOneShot(gritos[indexAleatorio]);
        }
    }
}