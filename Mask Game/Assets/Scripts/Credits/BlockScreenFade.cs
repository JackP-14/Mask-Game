using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackScreenFade : MonoBehaviour
{
    [Header("Configuración")]
    public Image cajaNegra;
    public float duracionFade = 2.0f;
    [Tooltip("Si es true, desactiva el objeto al terminar el fade in para que no moleste.")]
    public bool desactivarAlFinal = true;

    void Start()
    {
        SaveData.Instance.lives = 5;
        // AUTO-CORRECCIÓN: Si se te olvidó asignar la imagen, intentamos cogerla del propio objeto
        if (cajaNegra == null) cajaNegra = GetComponent<Image>();

        // --- COMPORTAMIENTO AUTOMÁTICO (FADE IN) ---
        // Esto se ejecuta SIEMPRE al iniciar la escena (lo que ya tenías)
        if (cajaNegra != null)
        {
            // 1. Forzamos negro opaco al inicio
            Color c = cajaNegra.color;
            c.a = 1f;
            cajaNegra.color = c;

            // Aseguramos que el objeto esté activo para verse
            cajaNegra.gameObject.SetActive(true);

            // 2. Arrancamos el fade hacia transparente
            StartCoroutine(HacerFadeDeNegroATransparente());
        }
    }

    // --- CORRUTINA 1: ENTRADA (De Negro a Transparente) ---
    // Esta es la privada que usa el Start automáticamente.
    IEnumerator HacerFadeDeNegroATransparente()
    {
        float tiempo = 0f;
        Color colorInicial = cajaNegra.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            float nuevaOpacidad = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            cajaNegra.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, nuevaOpacidad);
            yield return null;
        }

        // Final limpio: Transparente total
        cajaNegra.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, 0f);

        // Desactivamos para permitir clicks en el juego
        if (desactivarAlFinal)
        {
            gameObject.SetActive(false);
        }
    }

    // --- CORRUTINA 2: SALIDA (De Transparente a Negro) ---
    // Esta es PÚBLICA. No se ejecuta sola. Solo se ejecuta si el DialogueSystem la llama.
    // Esto asegura que no rompa otras escenas.
    public IEnumerator HacerFadeDeTransparenteANegro()
    {
        // 1. Reactivamos el objeto (porque seguramente se desactivó en el Start)
        gameObject.SetActive(true);

        float tiempo = 0f;
        Color colorBase = cajaNegra.color;

        // Aseguramos que empiece invisible (Alpha 0)
        cajaNegra.color = new Color(colorBase.r, colorBase.g, colorBase.b, 0f);

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            // Lerp inverso: de 0 a 1
            float nuevaOpacidad = Mathf.Lerp(0f, 1f, tiempo / duracionFade);
            cajaNegra.color = new Color(colorBase.r, colorBase.g, colorBase.b, nuevaOpacidad);
            yield return null;
        }

        // Final: Negro total
        cajaNegra.color = new Color(colorBase.r, colorBase.g, colorBase.b, 1f);
    }
}