using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // 1. IMPORTANTE: Necesario para el nuevo sistema

public class ImageCycler : MonoBehaviour
{
    [Header("Componentes")]
    public Image imagenEnPantalla; 

    [Header("Tus Sprites")]
    public Sprite[] formas; 

    private int index = 0; 

    void Start()
    {
        if (formas.Length > 0)
        {
            imagenEnPantalla.sprite = formas[0];
        }
    }

    void Update()
    {
        // 2. CORRECCIÓN: Usamos el sistema nuevo (Mouse.current)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            CambiarImagen();
        }
    }

    public void CambiarImagen()
    {
        // Solo intentamos cambiar si tenemos formas asignadas
        if (formas.Length == 0) return;

        index++;

        if (index >= formas.Length)
        {
            index = 0; // Vuelve al triángulo
        }

        imagenEnPantalla.sprite = formas[index];
    }
}