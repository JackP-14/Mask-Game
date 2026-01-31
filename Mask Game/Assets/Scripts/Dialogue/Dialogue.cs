using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // Necesario para manejar la Imagen de fondo
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    // --- NUEVA ESTRUCTURA DE DATOS ---
    // Esto define qué contiene cada "paso" del diálogo.
    [System.Serializable] // ¡Importante! Esto hace que aparezca en el Inspector
    public class LineaDeDialogo
    {
        [TextArea(3, 10)] // Hace que la caja de texto sea más grande en el Inspector
        public string texto;
        [Tooltip("Deja esto vacío si quieres mantener el fondo anterior.")]
        public Sprite imagenDeFondo; // La imagen opcional para esta frase
    }
    // ------------------------------------

    [Header("Componentes UI")]
    public TextMeshProUGUI textComponent;
    public Image fondoPantalla; // Arrastra aquí el objeto Image del fondo

    [Header("Configuración del Diálogo")]
    // En vez de string[], ahora usamos nuestra nueva estructura
    public LineaDeDialogo[] lineasDelDialogo; 
    public float textSpeed;

    private int index;
    private bool isTyping; 

    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    void Update()
    {
        // Input System: Click izquierdo
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!isTyping)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                // Accedemos al .texto de la estructura actual
                textComponent.text = lineasDelDialogo[index].texto;
                isTyping = false;
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        // Antes de empezar a escribir, comprobamos el fondo
        ActualizarFondo();
        StartCoroutine(TypeLine());
    }

    // --- NUEVA FUNCIÓN PARA CONTROLAR EL FONDO ---
    void ActualizarFondo()
    {
        // Miramos si la línea actual tiene una imagen asignada
        Sprite nuevaImagen = lineasDelDialogo[index].imagenDeFondo;

        // Si TIENE imagen, cambiamos el fondo.
        // Si es 'null' (está vacío en el inspector), no hacemos nada y se queda el anterior.
        if (nuevaImagen != null)
        {
            fondoPantalla.sprite = nuevaImagen;
        }
    }
    // ---------------------------------------------

    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty; 

        // Ahora recorremos el .texto de la estructura
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
            // Antes de escribir la siguiente línea, revisamos si toca cambio de fondo
            ActualizarFondo();
            StartCoroutine(TypeLine());
        }
        else
        {
            // Al terminar, cargamos la escena del juego
            SceneManager.LoadScene("Game");
        }
    }
}