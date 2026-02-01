// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI; // Necesario para manejar la Imagen de fondo
// using UnityEngine.InputSystem; 
// using UnityEngine.SceneManagement;

// public class Dialogue : MonoBehaviour
// {
//     // --- NUEVA ESTRUCTURA DE DATOS ---
//     // Esto define qué contiene cada "paso" del diálogo.
//     [System.Serializable] // ¡Importante! Esto hace que aparezca en el Inspector
//     public class LineaDeDialogo
//     {
//         [TextArea(3, 10)] // Hace que la caja de texto sea más grande en el Inspector
//         public string texto;
//         [Tooltip("Deja esto vacío si quieres mantener el fondo anterior.")]
//         public Sprite imagenDeFondo; // La imagen opcional para esta frase
//     }
//     // ------------------------------------

//     [Header("Componentes UI")]
//     public TextMeshProUGUI textComponent;
//     public Image fondoPantalla; // Arrastra aquí el objeto Image del fondo

//     [Header("Configuración del Diálogo")]
//     // En vez de string[], ahora usamos nuestra nueva estructura
//     public LineaDeDialogo[] lineasDelDialogo; 
//     public float textSpeed;

//     private int index;
//     private bool isTyping; 

//     void Start()
//     {
//         textComponent.text = string.Empty;
//         StartDialogue();
//     }

//     void Update()
//     {
//         // Input System: Click izquierdo
//         if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
//         {
//             if (!isTyping)
//             {
//                 NextLine();
//             }
//             else
//             {
//                 StopAllCoroutines();
//                 // Accedemos al .texto de la estructura actual
//                 textComponent.text = lineasDelDialogo[index].texto;
//                 isTyping = false;
//             }
//         }
//     }

//     void StartDialogue()
//     {
//         index = 0;
//         // Antes de empezar a escribir, comprobamos el fondo
//         ActualizarFondo();
//         StartCoroutine(TypeLine());
//     }

//     // --- NUEVA FUNCIÓN PARA CONTROLAR EL FONDO ---
//     void ActualizarFondo()
//     {
//         // Miramos si la línea actual tiene una imagen asignada
//         Sprite nuevaImagen = lineasDelDialogo[index].imagenDeFondo;

//         // Si TIENE imagen, cambiamos el fondo.
//         // Si es 'null' (está vacío en el inspector), no hacemos nada y se queda el anterior.
//         if (nuevaImagen != null)
//         {
//             fondoPantalla.sprite = nuevaImagen;
//         }
//     }
//     // ---------------------------------------------

//     IEnumerator TypeLine()
//     {
//         isTyping = true;
//         textComponent.text = string.Empty; 

//         // Ahora recorremos el .texto de la estructura
//         foreach (char c in lineasDelDialogo[index].texto.ToCharArray())
//         {
//             textComponent.text += c;
//             yield return new WaitForSeconds(textSpeed);
//         }
//         isTyping = false; 
//     }

//     void NextLine()
//     {
//         if (index < lineasDelDialogo.Length - 1)
//         {
//             index++;
//             textComponent.text = string.Empty;
//             // Antes de escribir la siguiente línea, revisamos si toca cambio de fondo
//             ActualizarFondo();
//             StartCoroutine(TypeLine());
//         }
//         else
//         {
//             // Al terminar, cargamos la escena del juego
//             SceneManager.LoadScene("Game");
//         }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DialogueSystemPro : MonoBehaviour
{
    [System.Serializable]
    public class LineaDeDialogo
    {
        [TextArea(3, 10)]
        public string texto;

        [Header("Visuales")]
        public Sprite imagenDeFondo; // Fondo estático
        public Sprite[] secuenciaAnimadaFondo; // Fondo animado (si lo usas)
        [Range(0.01f, 0.5f)]
        public float velocidadAnimacion = 0.1f;

        [Header("Sonido Específico (Sincronizado)")]
        [Tooltip("Este sonido sonará UNA vez justo al salir este texto.")]
        public AudioClip efectoDeSonido; 
    }

    [Header("Componentes UI")]
    public TextMeshProUGUI textComponent;
    public Image fondoPantalla;

    [Header("1. Audios de Fondo (Bucles Múltiples)")]
    [Tooltip("Mete aquí todos los sonidos que quieres que suenen de fondo a la vez (Música, Lluvia, Viento...).")]
    public AudioClip[] ambientesEnBucle; 

    [Header("2. Configuración del Diálogo")]
    public LineaDeDialogo[] lineasDelDialogo;
    public float textSpeed;

    // Variables internas
    private AudioSource sfxSource; // Fuente para los efectos sueltos
    private int index;
    private bool isTyping;
    private Coroutine animacionFondoActual;

    void Start()
    {
        // --- CONFIGURACIÓN AUTOMÁTICA DE AUDIO ---
        
        // 1. Crear la fuente para efectos sueltos (SFX)
        sfxSource = gameObject.AddComponent<AudioSource>();

        // 2. Crear fuentes para los ambientes en bucle (Múltiples capas)
        foreach (AudioClip clipAmbiente in ambientesEnBucle)
        {
            if (clipAmbiente != null)
            {
                // Creamos un "altavoz" nuevo por cada sonido de fondo para que suenen todos a la vez
                AudioSource ambienteSource = gameObject.AddComponent<AudioSource>();
                ambienteSource.clip = clipAmbiente;
                ambienteSource.loop = true;  // ¡Importante! Bucle infinito
                ambienteSource.volume = 0.5f; // Volumen al 50% por defecto para no saturar
                ambienteSource.Play();
            }
        }
        // -----------------------------------------

        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isTyping)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                if (animacionFondoActual == null) ActualizarFondo();
                textComponent.text = lineasDelDialogo[index].texto;
                isTyping = false;
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        ActualizarFondo();
        ReproducirEfectoTexto(); 
        StartCoroutine(TypeLine());
    }

    // --- FUNCIÓN QUE DISPARA EL SONIDO ESPECÍFICO ---
    void ReproducirEfectoTexto()
    {
        // Si la línea actual tiene un efecto asignado...
        if (lineasDelDialogo[index].efectoDeSonido != null)
        {
            // ...lo reproducimos una vez sin cortar lo demás
            sfxSource.PlayOneShot(lineasDelDialogo[index].efectoDeSonido);
        }
    }
    // -----------------------------------------------

    void ActualizarFondo()
    {
        if (animacionFondoActual != null)
        {
            StopCoroutine(animacionFondoActual);
            animacionFondoActual = null;
        }

        LineaDeDialogo lineaActual = lineasDelDialogo[index];

        if (lineaActual.secuenciaAnimadaFondo != null && lineaActual.secuenciaAnimadaFondo.Length > 0)
        {
            animacionFondoActual = StartCoroutine(AnimarFondoRoutine(lineaActual.secuenciaAnimadaFondo, lineaActual.velocidadAnimacion));
        }
        else if (lineaActual.imagenDeFondo != null)
        {
            fondoPantalla.sprite = lineaActual.imagenDeFondo;
        }
    }

    IEnumerator AnimarFondoRoutine(Sprite[] frames, float velocidad)
    {
        int frameIndex = 0;
        while (true)
        {
            fondoPantalla.sprite = frames[frameIndex];
            frameIndex = (frameIndex + 1) % frames.Length;
            yield return new WaitForSeconds(velocidad);
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        foreach (char c in lineasDelDialogo[index].texto.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    void NextLine()
    {
        if (index < lineasDelDialogo.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            ActualizarFondo();
            ReproducirEfectoTexto(); // Chequeamos si la nueva línea tiene sonido
            StartCoroutine(TypeLine());
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Introduction") {
                SceneManager.LoadScene("Game");
            // }
            // else if (SceneManager.GetActiveScene().name == "EndgameBad" || SceneManager.GetActiveScene().name == "EndgameGood" ||) {
            //     SceneManager.LoadScene("Credits");
            }
            else{
              SceneManager.LoadScene("Credits");  
            }
            
        }
    }
}