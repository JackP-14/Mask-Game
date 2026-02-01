using UnityEngine;
using UnityEngine.UI; // Necesario para controlar la Imagen
using System.Collections;

public class BlackScreenFade : MonoBehaviour
{
    [Header("Configuración")]
    public Image cajaNegra; // Arrastra aquí tu imagen negra
    public float duracionFade = 2.0f; // Cuánto tarda en desaparecer
    [Tooltip("Si es true, destruye o desactiva el objeto al terminar para que no moleste.")]
    public bool desactivarAlFinal = true;

    void Start()
    {
        // 1. Aseguramos que al empezar sea NEGRO TOTAL (Alpha 1)
        if (cajaNegra != null)
        {
            Color c = cajaNegra.color;
            c.a = 1f;
            cajaNegra.color = c;

            // 2. Arrancamos el fade
            StartCoroutine(HacerFadeDeNegroATransparente());
        }
    }

    IEnumerator HacerFadeDeNegroATransparente()
    {
        float tiempo = 0f;
        Color colorInicial = cajaNegra.color;

        while (tiempo < duracionFade)
        {
            tiempo += Time.deltaTime;
            
            // Calculamos la nueva opacidad (de 1 a 0)
            float nuevaOpacidad = Mathf.Lerp(1f, 0f, tiempo / duracionFade);
            
            // Aplicamos el color nuevo
            cajaNegra.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, nuevaOpacidad);
            
            yield return null;
        }

        // 3. Al terminar, nos aseguramos que sea invisible total
        cajaNegra.color = new Color(colorInicial.r, colorInicial.g, colorInicial.b, 0f);

        // 4. Desactivamos el objeto para que no bloquee los clicks del ratón
        if (desactivarAlFinal)
        {
            gameObject.SetActive(false);
        }
    }
}