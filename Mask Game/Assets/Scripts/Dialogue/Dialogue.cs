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