using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))] // Esto asegura que el objeto tenga AudioSource
public class MusicFadeIn : MonoBehaviour
{
    [Header("Configuración del Fade")]
    [Tooltip("¿Cuántos segundos tarda en subir el volumen?")]
    public float duracionDelFade = 3.0f; 

    [Tooltip("El volumen final al que llegará (de 0 a 1).")]
    [Range(0f, 1f)]
    public float volumenObjetivo = 1.0f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // 1. Preparamos el audio: Volumen a 0 y Play
        audioSource.volume = 0f;
        audioSource.Play();

        // 2. Arrancamos la subida gradual
        StartCoroutine(HacerFadeIn());
    }

    IEnumerator HacerFadeIn()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionDelFade)
        {
            tiempoTranscurrido += Time.deltaTime;
            
            // Calculamos el volumen proporcional al tiempo que ha pasado
            // Lerp hace una transición suave desde 0 hasta el objetivo
            audioSource.volume = Mathf.Lerp(0f, volumenObjetivo, tiempoTranscurrido / duracionDelFade);
            
            yield return null; // Esperamos al siguiente frame
        }

        // Nos aseguramos de que acabe EXACTAMENTE en el volumen objetivo
        audioSource.volume = volumenObjetivo;
    }
}