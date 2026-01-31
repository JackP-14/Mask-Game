using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextController : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI mainText; // El texto que cambia: "MÁSCARA" → "La máscara es: Grande"
    public TextMeshProUGUI questionPromptText; // El texto fijo "PREGUNTAR POR" o "La máscara es de color:"
    public Button previousButton; // AbajoButton (<)
    public Button nextButton; // ArribaButton (>)
    public Button selectButton; // Categoria/Preguntar
    public Button backButton; // Retrocede

    [Header("Categories")]
    public string[] categories = new string[]
    {
        "MÁSCARA",
        "COLOR",
        "OJOS",
        "ACCESORIO"
    };

    [Header("Questions per Category")]
    // Tipo de Máscara
    public string[] maskTypeAnswers = new string[]
    {
        "Grande",
        "Mediana",
        "Pequeña"
    };

    // Color
    public string[] colorAnswers = new string[]
    {
        "Rojo",
        "Azul",
        "Verde",
        "Amarillo",
        "Blanco"
    };

    // Ojos
    public string[] eyesAnswers = new string[]
    {
        "Despierto",
        "Muerto",
        "Inquieto",
        "Pícaro",
        "Araña"
    };

    // Accesorio
    public string[] accessoryAnswers = new string[]
    {
        "Pluma",
        "Flor",
        "Cuerno",
        "Rosario",
        "Sin accesorio"
    };

    private bool isSelectingCategory = true; // true = seleccionando categoría, false = seleccionando respuesta
    private int currentCategoryIndex = 0;
    private int currentAnswerIndex = 0;
    private string[] currentAnswers; // Array actual de respuestas

    void Start()
    {
        // Configurar listeners
        if (previousButton != null)
            previousButton.onClick.AddListener(OnPreviousButton);

        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButton);

        if (selectButton != null)
            selectButton.onClick.AddListener(OnSelectButton);

        // Empezar mostrando las categorías
        ShowCategorySelection();
    }

    #region Button Handlers

    private void OnPreviousButton()
    {
        if (isSelectingCategory)
        {
            PreviousCategory();
        }
        else
        {
            PreviousAnswer();
        }
    }

    private void OnNextButton()
    {
        if (isSelectingCategory)
        {
            NextCategory();
        }
        else
        {
            NextAnswer();
        }
    }

    private void OnSelectButton()
    {
        if (isSelectingCategory)
        {
            SelectCategory();
        }
        else
        {
            AskQuestion();
        }
    }

    public void OnBackButton()
    {
        ShowCategorySelection();
    }

    #endregion

    #region Category Selection

    private void ShowCategorySelection()
    {
        isSelectingCategory = true;
        UpdateDisplay();
    }

    private void PreviousCategory()
    {
        currentCategoryIndex--;

        if (currentCategoryIndex < 0)
        {
            currentCategoryIndex = categories.Length - 1;
        }

        UpdateDisplay();
    }

    private void NextCategory()
    {
        currentCategoryIndex++;

        if (currentCategoryIndex >= categories.Length)
        {
            currentCategoryIndex = 0;
        }

        UpdateDisplay();
    }

    private void SelectCategory()
    {
        Debug.Log($"Categoría seleccionada: {categories[currentCategoryIndex]}");

        // Cargar las respuestas para esta categoría
        LoadAnswersForCategory(currentCategoryIndex);

        // Cambiar a modo de selección de respuesta
        isSelectingCategory = false;
        currentAnswerIndex = 0;

        UpdateDisplay();
    }

    #endregion

    #region Answer Selection

    private void LoadAnswersForCategory(int categoryIndex)
    {
        switch (categoryIndex)
        {
            case 0: // MÁSCARA
                currentAnswers = maskTypeAnswers;
                break;
            case 1: // COLOR
                currentAnswers = colorAnswers;
                break;
            case 2: // OJOS
                currentAnswers = eyesAnswers;
                break;
            case 3: // ACCESORIO
                currentAnswers = accessoryAnswers;
                break;
            default:
                currentAnswers = new string[] { "Error" };
                break;
        }

        Debug.Log($"Cargadas {currentAnswers.Length} respuestas para: {categories[categoryIndex]}");
    }

    private void PreviousAnswer()
    {
        currentAnswerIndex--;

        if (currentAnswerIndex < 0)
        {
            currentAnswerIndex = currentAnswers.Length - 1;
        }

        UpdateDisplay();
    }

    private void NextAnswer()
    {
        currentAnswerIndex++;

        if (currentAnswerIndex >= currentAnswers.Length)
        {
            currentAnswerIndex = 0;
        }

        UpdateDisplay();
    }

    private void AskQuestion()
    {
        string selectedCategory = categories[currentCategoryIndex];
        string selectedAnswer = currentAnswers[currentAnswerIndex];

        Debug.Log($"¡PREGUNTA REALIZADA! Categoría: {selectedCategory}, Respuesta: {selectedAnswer}");

        // Aquí verificaremos con el cultista target
        CheckAnswer(currentCategoryIndex, currentAnswerIndex);

        // Volver a selección de categoría
        ShowCategorySelection();
    }

    #endregion

    #region Display Update

    private void UpdateDisplay()
    {
        if (isSelectingCategory)
        {
            // Modo: Selección de categoría
            if (questionPromptText != null)
            {
                questionPromptText.text = "PREGUNTAR POR";
            }

            if (mainText != null)
            {
                mainText.text = categories[currentCategoryIndex];
            }

            Debug.Log($"Mostrando categoría: {categories[currentCategoryIndex]}");
        }
        else
        {
            // Modo: Selección de respuesta
            if (questionPromptText != null)
            {
                questionPromptText.text = GetQuestionPrompt(currentCategoryIndex);
            }

            if (mainText != null)
            {
                mainText.text = currentAnswers[currentAnswerIndex];
            }

            Debug.Log($"Mostrando respuesta: {currentAnswers[currentAnswerIndex]}");
        }
    }

    private string GetQuestionPrompt(int categoryIndex)
    {
        switch (categoryIndex)
        {
            case 0: // MÁSCARA
                return "La máscara es:";
            case 1: // COLOR
                return "La máscara es de color:";
            case 2: // OJOS
                return "Los ojos son:";
            case 3: // ACCESORIO
                return "El accesorio es:";
            default:
                return "Pregunta:";
        }
    }

    #endregion

    #region Answer Verification

    [Header("Game Controller")]
    public GameController gameController; // Añade esta referencia al principio del script

    private void CheckAnswer(int categoryIndex, int answerIndex)
    {
        Debug.Log($"Verificando - Categoría: {categoryIndex}, Respuesta: {answerIndex}");

        // Verificar con el GameController
        if (gameController != null)
        {
            gameController.VerifyAnswer(categoryIndex, answerIndex);
        }
        else
        {
            Debug.LogError("GameController no asignado en TextController!");
        }
    }
}

#endregion