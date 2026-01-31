using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    [Header("References")]
    public TextController textController;
    public CultistRandomSpawn cultistSpawner;

    [Header("Game State")]
    public int lives = 3;

    [Header("History Display - 6 TextMeshPro")]
    public TextMeshProUGUI historyText1;
    public TextMeshProUGUI historyText2;
    public TextMeshProUGUI historyText3;
    public TextMeshProUGUI historyText4;
    public TextMeshProUGUI historyText5;
    public TextMeshProUGUI historyText6;

    // Array para acceder a los textos fácilmente
    private TextMeshProUGUI[] historyTexts;

    // Lista de todas las respuestas (correctas e incorrectas)
    private List<string> answerHistory = new List<string>();

    void Start()
    {
        // Inicializar el array de textos
        historyTexts = new TextMeshProUGUI[]
        {
            historyText1,
            historyText2,
            historyText3,
            historyText4,
            historyText5,
            historyText6
        };

        // Limpiar todos los textos al inicio
        ClearHistoryDisplay();
    }

    public void VerifyAnswer(int categoryIndex, int answerIndex)
    {
        CultistRandomizer targetCultist = cultistSpawner.GetTargetCultist();

        if (targetCultist == null)
        {
            Debug.LogError("No hay cultista objetivo!");
            return;
        }

        bool isCorrect = false;
        string categoryName = "";
        string answerName = "";

        // Verificar según la categoría
        switch (categoryIndex)
        {
            case 0: // MÁSCARA
                categoryName = "la máscara";
                answerName = textController.maskTypeAnswers[answerIndex];
                isCorrect = (targetCultist.maskTypeIndex == answerIndex);
                break;

            case 1: // COLOR
                categoryName = "el color";
                answerName = textController.colorAnswers[answerIndex];
                isCorrect = (targetCultist.colorIndex == answerIndex);
                break;

            case 2: // OJOS
                categoryName = "los ojos";
                answerName = textController.eyesAnswers[answerIndex];
                isCorrect = (targetCultist.maskEyesIndex == answerIndex);
                break;

            case 3: // ACCESORIO
                categoryName = "el accesorio";
                answerName = textController.accessoryAnswers[answerIndex];
                // "Sin accesorio" es el último elemento (índice 4)
                if (answerIndex == 4)
                {
                    isCorrect = (targetCultist.maskAccessoryIndex == -1);
                }
                else
                {
                    isCorrect = (targetCultist.maskAccessoryIndex == answerIndex);
                }
                break;
        }

        if (isCorrect)
        {
            OnCorrectAnswer(categoryName, answerName);
        }
        else
        {
            OnWrongAnswer(categoryName, answerName);
        }
    }

    private void OnCorrectAnswer(string category, string answer)
    {
        // Crear mensaje de respuesta correcta
        string message = $"{category} es {answer}";

        // Añadir a la lista de historial
        AddToHistory(message);

        Debug.Log($"✓ SÍ, {message}");
    }

    private void OnWrongAnswer(string category, string answer)
    {
        // RESTAR VIDA
        lives--;

        // Crear mensaje de respuesta incorrecta
        string message = $"{category} NO es {answer}";

        // Añadir a la lista de historial
        AddToHistory(message);

        Debug.Log($"✗ {message}");
        Debug.Log($"Vidas restantes: {lives}");

        // CONSECUENCIA: Si se acaban las vidas
        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void AddToHistory(string message)
    {
        // Añadir el nuevo mensaje a la lista
        answerHistory.Add(message);

        // Actualizar la visualización
        UpdateHistoryDisplay();
    }

    private void UpdateHistoryDisplay()
    {
        // Limpiar todos los textos primero
        ClearHistoryDisplay();

        // Calcular desde qué índice empezamos (para mostrar solo las últimas 6)
        int startIndex = Mathf.Max(0, answerHistory.Count - 6);

        // Mostrar las últimas 6 respuestas
        for (int i = 0; i < Mathf.Min(6, answerHistory.Count); i++)
        {
            int historyIndex = startIndex + i;

            if (historyTexts[i] != null)
            {
                // Numerar desde 1, mostrando el mensaje correspondiente
                historyTexts[i].text = $"{(i + 1)}. {answerHistory[historyIndex]}";
            }
        }
    }

    private void ClearHistoryDisplay()
    {
        foreach (TextMeshProUGUI text in historyTexts)
        {
            if (text != null)
            {
                text.text = "";
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("═══════════════════════════════");
        Debug.Log("        GAME OVER");
        Debug.Log("═══════════════════════════════");

        // Aquí más adelante añadirás: cargar escena, pausar juego, etc.
    }

    // Método público para obtener el historial completo (por si lo necesitas)
    public List<string> GetAnswerHistory()
    {
        return new List<string>(answerHistory);
    }
}
