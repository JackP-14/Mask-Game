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

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;

// public class DialogueSystemPro : MonoBehaviour
// {
//     [System.Serializable]
//     public class LineaDeDialogo
//     {
//         [TextArea(3, 10)]
//         public string texto;

//         [Header("Visuales")]
//         public Sprite imagenDeFondo; // Fondo estático
//         public Sprite[] secuenciaAnimadaFondo; // Fondo animado (si lo usas)
//         [Range(0.01f, 0.5f)]
//         public float velocidadAnimacion = 0.1f;

//         [Header("Sonido Específico (Sincronizado)")]
//         [Tooltip("Este sonido sonará UNA vez justo al salir este texto.")]
//         public AudioClip efectoDeSonido; 
//     }

//     [Header("Componentes UI")]
//     public TextMeshProUGUI textComponent;
//     public Image fondoPantalla;

//     [Header("1. Audios de Fondo (Bucles Múltiples)")]
//     [Tooltip("Mete aquí todos los sonidos que quieres que suenen de fondo a la vez (Música, Lluvia, Viento...).")]
//     public AudioClip[] ambientesEnBucle; 

//     [Header("2. Configuración del Diálogo")]
//     public LineaDeDialogo[] lineasDelDialogo;
//     public float textSpeed;

//     // Variables internas
//     private AudioSource sfxSource; // Fuente para los efectos sueltos
//     private int index;
//     private bool isTyping;
//     private Coroutine animacionFondoActual;

//     void Start()
//     {
//         // --- CONFIGURACIÓN AUTOMÁTICA DE AUDIO ---
        
//         // 1. Crear la fuente para efectos sueltos (SFX)
//         sfxSource = gameObject.AddComponent<AudioSource>();

//         // 2. Crear fuentes para los ambientes en bucle (Múltiples capas)
//         foreach (AudioClip clipAmbiente in ambientesEnBucle)
//         {
//             if (clipAmbiente != null)
//             {
//                 // Creamos un "altavoz" nuevo por cada sonido de fondo para que suenen todos a la vez
//                 AudioSource ambienteSource = gameObject.AddComponent<AudioSource>();
//                 ambienteSource.clip = clipAmbiente;
//                 ambienteSource.loop = true;  // ¡Importante! Bucle infinito
//                 ambienteSource.volume = 0.5f; // Volumen al 50% por defecto para no saturar
//                 ambienteSource.Play();
//             }
//         }
//         // -----------------------------------------

//         textComponent.text = string.Empty;
//         StartDialogue();
//     }

//     void Update()
//     {
//         if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
//         {
//             if (!isTyping)
//             {
//                 NextLine();
//             }
//             else
//             {
//                 StopAllCoroutines();
//                 if (animacionFondoActual == null) ActualizarFondo();
//                 textComponent.text = lineasDelDialogo[index].texto;
//                 isTyping = false;
//             }
//         }
//     }

//     void StartDialogue()
//     {
//         index = 0;
//         ActualizarFondo();
//         ReproducirEfectoTexto(); 
//         StartCoroutine(TypeLine());
//     }

//     // --- FUNCIÓN QUE DISPARA EL SONIDO ESPECÍFICO ---
//     void ReproducirEfectoTexto()
//     {
//         // Si la línea actual tiene un efecto asignado...
//         if (lineasDelDialogo[index].efectoDeSonido != null)
//         {
//             // ...lo reproducimos una vez sin cortar lo demás
//             sfxSource.PlayOneShot(lineasDelDialogo[index].efectoDeSonido);
//         }
//     }
//     // -----------------------------------------------

//     void ActualizarFondo()
//     {
//         if (animacionFondoActual != null)
//         {
//             StopCoroutine(animacionFondoActual);
//             animacionFondoActual = null;
//         }

//         LineaDeDialogo lineaActual = lineasDelDialogo[index];

//         if (lineaActual.secuenciaAnimadaFondo != null && lineaActual.secuenciaAnimadaFondo.Length > 0)
//         {
//             animacionFondoActual = StartCoroutine(AnimarFondoRoutine(lineaActual.secuenciaAnimadaFondo, lineaActual.velocidadAnimacion));
//         }
//         else if (lineaActual.imagenDeFondo != null)
//         {
//             fondoPantalla.sprite = lineaActual.imagenDeFondo;
//         }
//     }

//     IEnumerator AnimarFondoRoutine(Sprite[] frames, float velocidad)
//     {
//         int frameIndex = 0;
//         while (true)
//         {
//             fondoPantalla.sprite = frames[frameIndex];
//             frameIndex = (frameIndex + 1) % frames.Length;
//             yield return new WaitForSeconds(velocidad);
//         }
//     }

//     IEnumerator TypeLine()
//     {
//         isTyping = true;
//         textComponent.text = string.Empty;

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
//             ActualizarFondo();
//             ReproducirEfectoTexto(); // Chequeamos si la nueva línea tiene sonido
//             StartCoroutine(TypeLine());
//         }
//         else
//         {
//             if (SceneManager.GetActiveScene().name == "Introduction") {
//                 SceneManager.LoadScene("Game");
//             // }
//             // else if (SceneManager.GetActiveScene().name == "EndgameBad" || SceneManager.GetActiveScene().name == "EndgameGood" ||) {
//             //     SceneManager.LoadScene("Credits");
//             }
//             else{
//               SceneManager.LoadScene("Credits");  
//             }
            
//         }
//     }
// }


// using System.Collections;
// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;
// using UnityEngine.InputSystem;
// using UnityEngine.SceneManagement;

// public class DialogueSystemPro : MonoBehaviour
// {
//     [System.Serializable]
//     public class LineaDeDialogo
//     {
//         [TextArea(3, 10)]
//         public string texto;

//         [Header("Visuales")]
//         public Sprite imagenDeFondo; 
//         public Sprite[] secuenciaAnimadaFondo; 
//         [Range(0.01f, 0.5f)]
//         public float velocidadAnimacion = 0.1f;

//         [Header("Sonido Específico")]
//         public AudioClip efectoDeSonido; 
//     }

//     [Header("Referencias OBLIGATORIAS")]
//     public TextMeshProUGUI textComponent;
//     public Image fondoPantalla;
    
//     // --- ESTO ES LO QUE FALTABA PARA QUE FUNCIONE ---
//     [Tooltip("Arrastra aquí el objeto Canvas que tiene el script BlackScreenFade")]
//     public BlackScreenFade pantallaNegraScript; 
//     // ------------------------------------------------

//     [Header("1. Audios de Fondo")]
//     public AudioClip[] ambientesEnBucle; 

//     [Header("2. Configuración del Diálogo")]
//     public LineaDeDialogo[] lineasDelDialogo;
//     public float textSpeed = 0.05f;

//     // Variables internas
//     private AudioSource sfxSource;
//     private int index;
//     private bool isTyping;
//     private Coroutine animacionFondoActual;
//     private bool dialogoTerminado = false; // Candado para no repetir acciones

//     void Start()
//     {
//         // Setup de Audio
//         sfxSource = gameObject.AddComponent<AudioSource>();
//         if (ambientesEnBucle != null)
//         {
//             foreach (AudioClip clipAmbiente in ambientesEnBucle)
//             {
//                 if (clipAmbiente != null)
//                 {
//                     AudioSource ambienteSource = gameObject.AddComponent<AudioSource>();
//                     ambienteSource.clip = clipAmbiente;
//                     ambienteSource.loop = true;
//                     ambienteSource.volume = 0.5f;
//                     ambienteSource.Play();
//                 }
//             }
//         }

//         textComponent.text = string.Empty;
//         StartDialogue();
//     }

//     void Update()
//     {
//         // Si el diálogo terminó, bloqueamos el click para que no intente pasar de línea
//         if (dialogoTerminado) return;

//         if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
//         {
//             if (!isTyping)
//             {
//                 NextLine();
//             }
//             else
//             {
//                 StopAllCoroutines();
//                 if (animacionFondoActual == null) ActualizarFondo();
//                 textComponent.text = lineasDelDialogo[index].texto;
//                 isTyping = false;
//             }
//         }
//     }

//     void StartDialogue()
//     {
//         index = 0;
//         ActualizarFondo();
//         ReproducirEfectoTexto(); 
//         StartCoroutine(TypeLine());
//     }

//     void ReproducirEfectoTexto()
//     {
//         if (lineasDelDialogo[index].efectoDeSonido != null)
//         {
//             sfxSource.PlayOneShot(lineasDelDialogo[index].efectoDeSonido);
//         }
//     }

//     void ActualizarFondo()
//     {
//         if (animacionFondoActual != null)
//         {
//             StopCoroutine(animacionFondoActual);
//             animacionFondoActual = null;
//         }

//         LineaDeDialogo lineaActual = lineasDelDialogo[index];

//         if (lineaActual.secuenciaAnimadaFondo != null && lineaActual.secuenciaAnimadaFondo.Length > 0)
//         {
//             animacionFondoActual = StartCoroutine(AnimarFondoRoutine(lineaActual.secuenciaAnimadaFondo, lineaActual.velocidadAnimacion));
//         }
//         else if (lineaActual.imagenDeFondo != null)
//         {
//             fondoPantalla.sprite = lineaActual.imagenDeFondo;
//         }
//     }

//     IEnumerator AnimarFondoRoutine(Sprite[] frames, float velocidad)
//     {
//         int frameIndex = 0;
//         while (true)
//         {
//             fondoPantalla.sprite = frames[frameIndex];
//             frameIndex = (frameIndex + 1) % frames.Length;
//             yield return new WaitForSeconds(velocidad);
//         }
//     }

//     IEnumerator TypeLine()
//     {
//         isTyping = true;
//         textComponent.text = string.Empty;

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
//             ActualizarFondo();
//             ReproducirEfectoTexto();
//             StartCoroutine(TypeLine());
//         }
//         else
//         {
//             // AL TERMINAR EL DIÁLOGO, INICIAMOS LA SECUENCIA DE SALIDA
//             StartCoroutine(TerminarYCambiarEscena());
//         }
//     }

//     // --- AQUÍ ESTÁ LA SOLUCIÓN DEL FADE ---
//     IEnumerator TerminarYCambiarEscena()
//     {
//         dialogoTerminado = true; // Bloqueamos inputs

//         // 1. Decidimos a qué escena ir según donde estemos
//         string escenaActual = SceneManager.GetActiveScene().name;
//         string escenaDestino = "MainMenu"; // Por defecto, volvemos al menú

//         if (escenaActual == "Introduction") 
//         {
//             escenaDestino = "Game";
//         }
//         else if (escenaActual == "EndgameBad" || escenaActual == "EndgameGood")
//         {
//             escenaDestino = "Credits";
//         }
//         else if (escenaActual == "Credits")
//         {
//             escenaDestino = "MainMenu"; // Aseguramos que Credits vaya a MainMenu
//         }

//         // 2. HACEMOS EL FADE OUT Y ESPERAMOS
//         if (pantallaNegraScript != null)
//         {
//             // Llamamos a la función pública del otro script y esperamos a que acabe
//             yield return StartCoroutine(pantallaNegraScript.HacerFadeDeTransparenteANegro());
//         }
//         else
//         {
//             Debug.LogError("¡OJO! No has asignado la 'Pantalla Negra Script' en el Inspector. Cambio brusco.");
//         }

//         // 3. CARGAMOS LA ESCENA
//         SceneManager.LoadScene(escenaDestino);
//     }
// }


using System.Collections;
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
        public Sprite imagenDeFondo; 
        public Sprite[] secuenciaAnimadaFondo; 
        [Range(0.01f, 0.5f)]
        public float velocidadAnimacion = 0.1f;

        [Header("Sonido Específico")]
        public AudioClip efectoDeSonido; 
    }

    [Header("Referencias OBLIGATORIAS")]
    public TextMeshProUGUI textComponent;
    public Image fondoPantalla;
    
    [Tooltip("Arrastra aquí el objeto Canvas que tiene el script BlackScreenFade")]
    public BlackScreenFade pantallaNegraScript; 

    [Header("1. Audios de Fondo")]
    public AudioClip[] ambientesEnBucle; 

    [Header("2. Configuración del Diálogo")]
    public LineaDeDialogo[] lineasDelDialogo;
    public float textSpeed = 0.05f;

    // Variables internas
    private AudioSource sfxSource;
    private int index; // Línea actual que estamos viendo
    private int maxIndexReached; // La línea más avanzada a la que ha llegado el jugador
    private bool isTyping;
    private Coroutine animacionFondoActual;
    private bool dialogoTerminado = false;

    void Start()
    {
        // Setup de Audio
        sfxSource = gameObject.AddComponent<AudioSource>();
        if (ambientesEnBucle != null)
        {
            foreach (AudioClip clipAmbiente in ambientesEnBucle)
            {
                if (clipAmbiente != null)
                {
                    AudioSource ambienteSource = gameObject.AddComponent<AudioSource>();
                    ambienteSource.clip = clipAmbiente;
                    ambienteSource.loop = true;
                    ambienteSource.volume = 0.5f;
                    ambienteSource.Play();
                }
            }
        }

        textComponent.text = string.Empty;
        index = 0;
        maxIndexReached = 0; // Al empezar, estamos en la 0
        MostrarLineaActual();
    }

    void Update()
    {
        if (dialogoTerminado) return;

        // 1. INPUT DE CLICK (Avanzar)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isTyping)
            {
                // Solo avanzamos si estamos en la última línea alcanzada
                if (index == maxIndexReached)
                {
                    NextLine();
                }
                else
                {
                    // Si estábamos revisando el historial y hacemos click,
                    // volvemos instantáneamente al presente (Opcional, o avanzar 1 a 1)
                    // Aquí lo configuro para avanzar 1 a 1 en el historial también:
                    AvanzarEnHistorial();
                }
            }
            else
            {
                // Completar texto instantáneo
                StopAllCoroutines();
                if (animacionFondoActual == null) ActualizarFondo();
                textComponent.text = lineasDelDialogo[index].texto;
                isTyping = false;
            }
        }

        // 2. INPUT DE RUEDA (Scroll)
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.y.ReadValue();

            if (scroll > 0) // SCROLL UP -> IR ATRÁS
            {
                RetrocederEnHistorial();
            }
            else if (scroll < 0) // SCROLL DOWN -> IR ADELANTE
            {
                AvanzarEnHistorial();
            }
        }
    }

    // --- LÓGICA DE NAVEGACIÓN ---

    void RetrocederEnHistorial()
    {
        if (index > 0)
        {
            index--;
            MostrarLineaActual();
        }
    }

    void AvanzarEnHistorial()
    {
        // Solo podemos avanzar si NO estamos escribiendo y NO hemos llegado al final real
        if (!isTyping)
        {
            // Si estamos revisando el pasado (index < maxIndexReached), avanzamos 1
            if (index < maxIndexReached)
            {
                index++;
                MostrarLineaActual();
            }
            // Si estamos en el presente, intentamos avanzar a una línea nueva
            else if (index == maxIndexReached)
            {
                NextLine();
            }
        }
    }

    void NextLine()
    {
        if (index < lineasDelDialogo.Length - 1)
        {
            index++;
            maxIndexReached = index; // ¡Actualizamos el progreso máximo!
            MostrarLineaActual();
        }
        else
        {
            StartCoroutine(TerminarYCambiarEscena());
        }
    }

    // --- FUNCIÓN CENTRAL PARA ACTUALIZAR TODO ---
    void MostrarLineaActual()
    {
        // 1. Limpieza
        StopAllCoroutines(); 
        textComponent.text = string.Empty;
        
        // 2. Actualizar Visuales y Audio
        ActualizarFondo();
        
        // Solo reproducimos el sonido si estamos avanzando o es la primera vez,
        // para que al hacer scroll rápido hacia atrás no suene todo de golpe.
        // (O quita el 'if' si quieres que suene siempre)
        ReproducirEfectoTexto(); 

        // 3. Escribir texto
        StartCoroutine(TypeLine());
    }
    // -------------------------------------------

    void ReproducirEfectoTexto()
    {
        // Detenemos el anterior para que no se solapen si haces scroll rápido
        sfxSource.Stop(); 

        if (lineasDelDialogo[index].efectoDeSonido != null)
        {
            sfxSource.PlayOneShot(lineasDelDialogo[index].efectoDeSonido);
        }
    }

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
        
        // Si quieres que al volver atrás el texto salga instantáneo (típico rollback),
        // descomenta este bloque y comenta el foreach:
        /*
        if (index < maxIndexReached) {
             textComponent.text = lineasDelDialogo[index].texto;
             isTyping = false;
             yield break;
        }
        */

        foreach (char c in lineasDelDialogo[index].texto.ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    IEnumerator TerminarYCambiarEscena()
    {
        dialogoTerminado = true; 

        string escenaActual = SceneManager.GetActiveScene().name;
        string escenaDestino = "MainMenu"; 

        if (escenaActual == "Introduction") 
        {
            escenaDestino = "Game";
        }
        else if (escenaActual == "Game" || escenaActual == "EndgameBad" || escenaActual == "EndgameGood")
        {
            escenaDestino = "Credits";
        }
        else if (escenaActual == "Credits")
        {
            escenaDestino = "MainMenu";
        }

        if (pantallaNegraScript != null)
        {
            yield return StartCoroutine(pantallaNegraScript.HacerFadeDeTransparenteANegro());
        }
        else
        {
            Debug.LogError("¡No has asignado la 'Pantalla Negra Script'!");
        }

        SceneManager.LoadScene(escenaDestino);
    }
}